## 팀 프로젝트 기술 문서

### 1. **폴더 구조 확인**

```
LibrarySystem/
│
├──   Controllers/                  #  [제어 계층] 시스템 전반의 흐름 및 요청 제어
│   ├── AuthController.cs            # 로그인/로그아웃 및 사서·유저 권한 제어
│   ├── LibrarianController.cs       # [사서 전용], 연체자 목록 관리
│   ├── UserController.cs            # [유저 전용] 도서 대출 및 반납(ISBN)
│   └── CurationController.cs        # [유저 전용] 대출 이력 기반 도서 추천
│
├──   Models/                       #  [데이터 계층] DB 테이블과 1:1 매핑되는 엔티티 
│   ├── User.cs                      # 사용자 정보 (ID, PW, 권한)
│   └── LoanRecord.cs                # 대출 이력 (UserID, ISBN, 대출/반납일)
│
├──   Views/                        #  [UI 계층] 사용자에게 보여지는 Windows Forms 화면
│   ├──   Auth/                     # 로그인 폼 화면 
│   ├──   Librarian/                # 연체자 관리 대시보드 화면
│   ├──   User/                     # 대출 및 반납 폼 화면
│   └──   Curation/                 # 분야별 독서 차트 및 추천 도서 표시 화면
│
└──   Services/                     #  [비즈니스 로직 계층] 핵심 처리 및 외부 API 연동
    ├── AuthService.cs               # DB 기반 로그인 및 권한 부여 로직
    ├── LibraryService.cs            # 대출/반납 승인, 연체자 상태 업데이트 로직
    ├── AladinApiService.cs          # [알라딘 API] 베스트셀러, 신간 도서 추천, 도서 검색 기능
    └── RecommendationService.cs     # 도서 추천 알고리즘 (유저 대출 이력 분석 및 교차 추천)
```

---

### 1. Models (데이터 계층)

- **`User.cs`**
    - **기능:** 시스템 사용자(사서 및 유저)의 기본 정보를 담고 있는 모델 및 데이터베이스 연동 클래스
    - **구현 속성:** `UserId` (DB 자동 PK), `UserLoginId` (로그인 아이디), `Password` (비밀번호), `Name` (이름), `Role` (권한: Member/Admin), `Email` (이메일), `PreferCategory` (선호 도서 분야)
    - **메서드:**
        - `InsertUser()`: 현재 객체의 정보를 바탕으로 데이터베이스에 새로운 유저로 회원가입(저장)을 수행합니다.
        - `GetUser(string userLoginId)`: 아이디로 데이터베이스를 조회 후 해당 정보를 담은 User 객체 반환.
        - `GetAllUsers()`: 등록된 모든 유저를 리스트로 반환.
- **`LoanRecord.cs`**
    - **기능:** 사용자의 도서 대출 및 반납 이력을 기록한 db
    - **구현 속성:** `LoanId` (키), `UserId` (외래키?), `Isbn` (대출한 책의 고유번호), `LoanDate` (대출일), `ReturnDate` (반납일, null이면 대출 중).


### 2. Services 
- **`AuthService.cs`**
    - **기능:** 입력받은 정보가 DB의 `User` 테이블과 일치하는지 검증합니다.
    - **메서드:** `Authenticate(string userId, string password)` → 성공 시 해당 유저의 권한(Role)을 반환.
    
    
- **`LibraryService.cs`**
    - **기능:** 대출/반납 처리하고 연체 상태를 계산합니다. DB의 `LoanRecord`를 조작합니다.
    - **메서드:** - `LoanBook(string userId, string isbn)`: 대출 기록 생성
        - `ReturnBook(string userId, string isbn)`: 해당 ISBN의 `ReturnDate`를 현재 날짜로 업데이트.
        - `GetOverdueUsers()`: 반납일이 지났는데 `ReturnDate`가 null인 유저 목록 조회.


- **`AladinApiService.cs`**
    - **기능:** 책 상세 정보를 가져오거나 추천을 위한 리스트를 호출합니다.
    - **메서드:**
        - `GetBookDetails(string isbn)`: 책 제목, 저자, 카테고리 정보, isbn 번호 등 필요 정보 호출.
        - `GetRecommendations(string categoryId)`: 특정 카테고리의 베스트셀러나 신간 호출.
        - `SearchBooks(string book)`: 책 제목으로 제목, 저자, 카테고리 정보, isbn 번호 출력 기능.
        - `AladinApiPreprocess()`: 알라딘 API의 결과를 인식 가능한 형태로 변환하는 유틸리티 메서드.
        
        
- **`RecommendationService.cs (중간 이후 구현 예정)`**
    - **기능:** 유저의 대출 이력을 분석, 맞춤형 도서 추천


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
        5. 회원 권한이 'Member' 등 일반 유저일 경우: Auth 창을 숨기고 `UserController` 인스턴스 생성 및 일반 유저 뷰로 전환

- **`UserController.cs` (사용자 컨트롤러)**
    - **기능:** 일반 유저용 화면 `UserView`를 렌더링하며 도서 검색, 대출/반납, 추천 도서 조회 등 유저가 수행하는 모든 화면 이벤트를 중계합니다.
    - **상세 흐름:** 
        - **화면 초기화:** `UserView`를 띄우면서 View의 이벤트(`SearchRequested`, `CurationRequested`) 구독. 외부 API인 `AladinApiService` 컨트롤러 내부 주입.
        - **검색 로직:** 
            1. 유저가 View에서 검색어 입력 후 '검색' 클릭
            2. `SearchRequested` 이벤트 발생, Controller가 트리거됨
            3. `AladinApiService.GetBooksByQuery(query)`를 비동기로 호출하여 알라딘 도서망 API와 통신
            4. 서버 결괏값(`List<BookItem>`)을 수신하여 `UserView.DisplayBooks()`의 인자로 넘겨주어 화면(DataGridView)에 알라딘 검색 결과를 실시간 렌더링하도록 명령.
        - **화면 전환 로직:** '추천 도서 보기' 클릭 시 `CurationRequested` 이벤트 수신 후 `CurationController` 혹은 `Curation` View를 호출하여 화면 전환.
        - *(추후 추가 예정)* ISBN 기반 대출/반납 이벤트를 수신하여 `LibraryService.LoanBook/ReturnBook` 제어.

- **`LibrarianController.cs` (사서 대시보드 제어)**
    - **기능:** 도서관 관리자 화면 기능 및 대시보드를 캡슐화합니다.
    - **상세 흐름:** 
        1. `AuthController`에 의해 호출되어 `Librarian` View 렌더링
        2. *(추후 추가 예정)* 폼 로드 이벤트를 구독하여 폼 로딩 시 `LibraryService.GetOverdueUsers()`를 호출
        3. *(추후 추가 예정)* 반환받은 연체자 목록 리스트를 View의 표(DataGridView 등)에 공급하여 시각적으로 최신화 지시.

- **`CurationController.cs` (도서 추천 화면 제어)**
    - **기능:** 통계 차트 및 사용자 대출 패턴 기반 추천 도서 목록을 준비합니다.
    - **상세 흐름:** 유저가 추천 탭 클릭 시 이벤트 전환 → `RecommendationService`에서 차트용 데이터 및 분야별 도서 리스트 연산 요청 → Curation View에 통계/추천 데이터 렌더링.

---
