## 팀 프로젝트 기술 문서
# Library Project (도서 관리 시스템)

### 주요 구현 기능 목록
- **회원 관리 시스템**: 사용자 회원가입, 로그인 및 유저(Member)/사서(Admin) 권한별 대시보드 화면 분기
- **도서 검색 및 DB 영속성**: 알라딘 OpenAPI를 활용한 실시간 도서 검색 및 대출 과정에서의 도서 DB 자동 캐싱
- **도서 대출 및 반납 관제**: 로그인 유저별 도서 대출 및 반납 상태 갱신, 이전 대출 내역 DB 연동 및 최신화
- **사용자 맞춤 도서 큐레이션**:
  - 대출 이력 기반의 도서 선호 카테고리 분포 현황 파이 차트 제공
  - 가장 최근에 대출한 도서의 저자 키워드 기반 연관 도서, 베스트셀러 교차 추천
- **연체자 관리 대시보드 (사서 전용)**: 반납 기한이 지났으나 미반납된 연체 도서 목록 조회, 연체자 대상 이메일 알림 발송용 리스트 추출

---

### 1. **폴더 구조 확인**

```
LibraryProject/
│
├── Controllers/                  # [제어 계층] 시스템 전반의 흐름 및 요청 제어
│   ├── AuthController.cs         # 로그인/로그아웃 및 권한별 화면 분기 제어
│   ├── CurationController.cs     # [유저 전용] 대출 이력 기반 도서 추천 뷰 제어
│   ├── LibrarianController.cs    # [사서 전용] 연체자 목록 관리 및 이메일 알림 발송
│   └── UserController.cs         # [유저 전용] 도서 검색, 대출, 반납 및 DB 연동 제어
│
├── Models/                       # [데이터 계층] DB 테이블과 1:1 매핑되는 로직 및 객체
│   ├── Book.cs                   # 시스템 도서 정보 객체 (DB 등록 및 캐싱)
│   ├── Database.cs               # 데이터베이스 연결 구성 관리
│   ├── LoanRecord.cs             # 대출/반납 이력 및 연체 데이터 모델
│   └── User.cs                   # 사용자 정보 (기본키, PW, 이메일 등) 및 계정 관련 모델
│
├── Services/                     # [비즈니스 로직 계층] 도서 추천 및 외부 API 연동
│   ├── AladinApiService.cs       # [알라딘 API] 신간 도서 조회 및 키워드/ISBN 도서 조회 연동
│   ├── LibraryService.cs         # 비회원 도서 대출 및 반납 상태 관리(메모리)
│   └── RecommendationService.cs  # 사용자 최신 대출 이력을 분석한 도서 교차 추천 알고리즘
│
└── Views/                        # [UI 계층] 사용자에게 보여지는 Windows Forms 화면
    ├── Auth.cs                   # 로그인 폼 화면
    ├── Curation.cs               # 분야별 통계 차트 및 AI 도서 추천 대시보드 폼
    ├── Librarian.cs              # 시스템 사서(Admin) 전용 연체자 관리 대시보드 폼
    ├── SignUp.cs                 # 신규 회원가입 폼 화면
    └── User.cs                   # 유저 도서 검색 및 대출/반납 내역 확인 폼
```

---

### 1. Models (데이터 계층)

- **`User.cs`**
    - **기능:** 시스템 사용자(사서 및 유저)의 기본 정보를 담고 있는 모델 및 데이터베이스 연동 클래스
    - **구현 속성:** `UserId` (DB 기본키), `UserLoginId` (로그인 아이디), `Password` (비밀번호), `Name` (이름), `Role` (권한: Member/Admin), `Email` (이메일), `PreferCategory` (선호 도서 분야)
    - **메서드:**
        - `InsertUser()`: 객체 정보를 바탕으로 데이터베이스에 새로운 유저 등록.
        - `GetUser(string userLoginId)`: 아이디로 데이터베이스를 조회 후 해당 정보를 담은 User 객체 반환.
        - `GetUserById(int userId)`: 고유 아이디(`UserId`)로 유저를 단건 조회 (연체자 이메일 조회 등에 활용).
- **`LoanRecord.cs`**
    - **기능:** 실제 사용자의 도서 대출 및 반납 이력, 연체 상태를 기록하고 반환하는 모델. 책 데이터와 JOIN 연산을 수행합니다.
    - **메서드:**
        - `InsertLoan(...)`: 대출 등록 시 도서를 우선 캐싱한 뒤 현재 날짜로 대출 기록 저장.
        - `GetLoansByUser(int userId)`: 특정 유저의 전체 대출/반납 이력 최신순 반환.
        - `GetOverdueLoans()`: 반납되지 않고 기한(`DueDate`)이 지난 대출 데이터 반환 (사서 대시보드용).
        - `ReturnBook(int loanId)`: 해당 대출 기록의 `ReturnDate`를 현재 날짜 시간으로 갱신하여 반납 처리.
- **`Book.cs`**
    - **기능:** 검색 및 대출 시 도서 데이터를 도서관 DB에 임시/영구 캐싱하는 클래스.
    - **메서드:**
        - `InsertBook()`: 알라딘 API로 받아온 도서를 시스템 DB에 저장. (`INSERT IGNORE`로 중복 방지)
        - `GetBook(string isbn13)`: 시스템 DB 내 등록된 도서 정보 단건 조회.
- **`Database.cs`**
    - **기능:** `config.json`의 정보를 읽어와 MySql 등 DB 서버와의 Connection 연결 객체를 생성해 반환합니다.


### 2. Services 

- **`LibraryService.cs`**
    - **기능:** 메인 DB 연결 전, 비회원이거나 메모리 환경에서 도서 모의 대출/반납 이력을 추적하도록 하는 인메모리 관리 서비스.
    - **메서드:** 
        - `LoanBook(BookItem book)`, `ReturnBook(string isbn13)`: 리스트 기반 임시 대출/반납 처리.
        - `GetCurrentLoans()`, `GetLoanHistory()`: 서비스가 보유한 현재 대출 및 누적 이력 반환.

- **`AladinApiService.cs`**
    - **기능:** 알라딘 외부 검색망 API와 비동기 HTTP 통신을 수행하며, 책 정보를 DTO(`BookItem`)로 변환합니다.
    - **메서드:**
        - `GetBookInfoAsync(long isbn13)`: DB에 저장된 카테고리 ID 값을 실제 카테고리명 텍스트로 보완하기 위해 ISBN 기반 단건 상세 조회 시 사용.
        - `GetBooksByQuery(string query)`: 사용자의 검색어로 제목, 저자, ISBN 등의 검색 목록 반환.
        - `GetBestsellersAsync(...)`: 특정 추천 카테고리의 베스트셀러 및 신간 호출.

- **`RecommendationService.cs`**
    - **기능:** 유저의 실제 대출 이력을 분석하여 맞춤형 도서 리스트를 계산합니다.
    - **상세 흐름:** 사용자 이력 중 가장 마지막으로 대출한 기록(최신 대출 이력)을 찾아 저자의 이름을 키워드로 알라딘 API에 검색을 요청하고, 본인이 이미 빌렸던 도서를 제외한 유사/교차 도서(없을 시 베스트셀러 등 데이터 폴백)를 추천 도서 목록으로 반환합니다.


### 3. Views (화면 관련 로직)

- `Auth/` 
    - `Auth.cs`: 진입 로그인 폼 화면. 유저로부터 정보를 입력받아 `LoginRequested`, `SignUpRequested` 이벤트를 발생시킵니다.
    - `SignUp.cs`: 회원가입 폼 화면. 새 회원 정보를 입력받아 `SignUpSubmitted` 이벤트를 발생시킵니다.
- `Librarian/`
    - `Librarian.cs`: 사서 전용 대시보드 화면. 연체자 목록 조회와 관리를 위한 UI.
- `User/` 
    - `User.cs`: 도서 대출 및 반납, 검색 기능을 위한 탭 화면 UI. 도서 검색, 검색 결과 조회, 나의 대출 목록 표시 등을 지원하며 Controller와 렌더링을 위임받거나 이벤트를 퍼블리싱 합니다.
- `Curation/` 
    - `Curation.cs`: 도서 분야별 차트 및 추천 도서 표시 화면. 전달받은 대출 이력 데이터를 기반으로 통계 차트를 그리거나 비동기로 받아온 추천 목록을 그립니다.



### 4. Controllers (구현 진행 중)

현재 프로젝트는 View 내부에 존재하던 비즈니스 로직과 화면 전환 로직을 Controller로 완전히 위임하는 **이벤트 기반 MVC(Model-View-Controller) 아키텍처**로 개편되어 있습니다. 
View(화면)에서 발생한 사용자 입력 이벤트를 Controller가 구독하고, Controller가 Service 로직을 호출한 뒤, 처리된 결과 데이터를 다시 View의 메서드를 통해 렌더링하는 형태로 데이터 흐름이 일방향으로 유지됩니다.

#### 전체적인 동작 플로우 메커니즘
1. **[View]** 사용자가 UI 요소(버튼, 텍스트박스 등) 조작
2. **[View]** 자체적인 로직 처리 없이, 선언된 `Event` (예: `LoginRequested`, `SearchRequested`)를 발생시킴 (입력된 데이터를 이벤트 인자로 함께 전달)
3. **[Controller]** 해당 View 초기화 당시 구독해 둔 이벤트 핸들러가 트리거됨
4. **[Controller]** 이벤트로 전달받은 데이터를 검증하고, 필요한 `Service` (DB 조회, API 통신 등) 객체의 메서드를 호출
5. **[Service]** 비즈니스 로직 수행 및 데이터 모델 가공 후 응답
6. **[Controller]** Service로부터 반환된 데이터를 판단하여 분기 처리 (다른 View 열기, 에러 메시지 출력 등) 혹은 현재 View의 Public 렌더링 메서드(예: `DisplayBooks()`)로 데이터를 주입하여 위임
7. **[View]** 주입된 데이터를 단순히 UI 컴포넌트(DataGridView 등)에 바인딩하여 화면 갱신

---

#### 각 Controller 기능 및 상세 흐름

- **`AuthController.cs` (로그인 및 회원가입 제어)**
    - **기능:** 시스템의 시작점으로써 사용자 로그인과 신규 회원가입을 제어하며, 확인된 권한에 맞춰 화면을 분기합니다.
    - **상세 흐름:** 
        1. 시스템 구동 시 `Auth` View(로그인 창) 인스턴스 생성 빛 애플리케이션 실행
        2. 화면에서 회원가입 요청(`SignUpRequested`)이 오면 `SignUp` View를 띄우고, 회원가입 제출(`SignUpSubmitted`)이 발생하면 새 `User` 객체를 생성해 DB에 추가(`InsertUser`)
        3. 로그인 요청(`LoginRequested`) 시 넘겨받은 아이디를 기반으로 `User.GetUser(id)`를 호출해 데이터를 확보하고 비밀번호 검증
        4. 회원 권한이 'Admin' 일 경우: Auth 창을 숨기고 `LibrarianController` 인스턴스 생성 및 사서 뷰로 전환
        5. 회원 권한이 'Member' 등 일반 유저일 경우: Auth 창을 숨기고 현재 로그인된 `User` 정보를 담아 `UserController` 인스턴스 생성 및 일반 유저 뷰로 전환

- **`UserController.cs` (사용자 컨트롤러)**
    - **기능:** 일반 유저용 화면 `UserView`를 렌더링하며 도서 검색, 대출/반납, 추천 도서 조회 등 유저가 수행하는 모든 화면 이벤트를 중계합니다. 로그인된 유저 정보가 있을 시 도서 대출/반납 내역을 데이터베이스와 연동합니다.
    - **상세 흐름:** 
        - **화면 초기화:** `UserView`를 띄우고 이벤트(`SearchRequested`, `CurationRequested`, `LoanRequested`, `ReturnRequested`)를 구독하며, 현재 생성된 `User`가 존재하면 DB(`LoanRecord.GetLoansByUser`)에서 대출 이력을 불러와 화면에 표시합니다.
        - **검색 로직:** 
            1. 유저가 View에서 검색어 입력 후 '검색' 클릭
            2. `SearchRequested` 이벤트 발생, Controller가 트리거됨
            3. `AladinApiService.GetBooksByQuery(query)`를 비동기로 호출하여 알라딘 도서망 API와 통신
            4. 서버 결괏값(`List<BookItem>`)을 수신하여 `UserView.DisplayBooks()`의 인자로 넘겨주어 화면(DataGridView)에 알라딘 검색 결과를 실시간 렌더링하도록 명령.
        - **화면 전환 로직:** '추천 도서 보기' 클릭 시 `CurationRequested` 이벤트 수신 후 `CurationController`에 대출 내역 및 `_user` 정보를 넘겨 호출.
        - **도서 대출/반납 로직:** `LoanRequested`, `ReturnRequested` 이벤트 발생 시 뷰에서 전달받은 도서 정보를 데이터베이스의 `LoanRecord` 및 `Book` 모델을 통해 실제로 저장, 반납(ReturnDate 갱신) 요청을 한 후, 뷰의 목록을 최신화합니다.

- **`LibrarianController.cs` (사서 대시보드 제어)**
    - **기능:** 도서관 관리자 화면 기능 및 대시보드를 캡슐화하고 연체자 알림 발송을 담당합니다.
    - **상세 흐름:** 
        1. `AuthController`에 의해 호출되어 `Librarian` View 렌더링 및 `LoanRecord.GetOverdueLoans()` 결과를 호출해 데이터그리드뷰에 표시.
        2. 연체 알림 발송 버튼 클릭 시 `NotifyRequested` 이벤트를 수신하여 연체 중인 유저의 고유 `UserId`들을 기반으로 `Model.User.GetUserById`를 호출
        3. 반환받은 유저 정보에서 이메일을 추출하여 알림 메시지 발송 목록을 가공하고 View를 통해 출력합니다.

- **`CurationController.cs` (도서 추천 화면 제어)**
    - **기능:** 통계 차트 및 사용자 대출 패턴 기반 추천 도서 목록을 준비합니다.
    - **상세 흐름:** 
        1. `UserController`로부터 넘겨받은 현재 유저(`_user`) 정보를 바탕으로 DB에 저장된 실제 사용자 대출 이력을 조회합니다.
        2. 데이터베이스에 카테고리값이 고유 INT Id로 저장되어 있으므로, 뷰로 넘기기 전 비동기적으로 도서 API(`AladinApiService`)를 다시 호출해 실제 텍스트 형태의 카테고리(`CategoryName`)로 값을 매핑합니다.
        3. 가공된 통계용 데이터를 `Curation` View에 전달(`DisplayStatistics`)하여 파이 차트를 그리도록 지시합니다.
        4. 해당 이력을 기반으로 `RecommendationService`에서 가장 최근 대출한 도서를 추출하고, 작가 이름 기준 교차 추천 도서 리스트를 연산해 뷰에 렌더링(`DisplayRecommendations`)합니다.

---
