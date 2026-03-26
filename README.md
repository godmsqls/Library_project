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
    - **기능:** 시스템 사용자(사서 및 유저)의 기본 정보 담은 db
    - **구현 속성:** `UserId` (키), `Password` (비밀번호), `Role` (사서/유저 구분용).
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
- `Librarian/`
- `User/` 
- `Curation/` 



### 4. Controllers (중간 이후 구현 예정)
- View(화면)에서 발생한 이벤트를 Service로 전달, Service의 결과값을 다시 View로 전달할 것.

- **`AuthController.cs`**
    - **기능:** 로그인 버튼 클릭 이벤트를 처리하고 화면을 전환합니다.
    - **흐름:** View에서 ID/PW 전달받음 → `AuthService`에 검증 요청 → 사서면 `Librarian` 폼 열기, 유저면 `User` 폼 열기.
    
    
- **`LibrarianController.cs`**
    - **기능:** 사서 대시보드의 기능을 제어합니다.
    - **흐름:** 폼이 로드될 때 `LibraryService.GetOverdueUsers()`를 호출하여 연체자 데이터를 View에 전달.


- **`UserController.cs`**
    - **기능:** 유저의 대출/반납 요청을 처리합니다.
    - **흐름:** View에서 ISBN 입력 후 버튼 클릭 → `LibraryService.LoanBook/ReturnBook` 호출 → 성공/실패 메시지 View에 전달.


- **`CurationController.cs`**
    - **기능:** 통계 및 추천 화면을 구성하기 위한 데이터를 준비합니다.
    - **흐름:** 유저가 추천 탭 클릭 → `RecommendationService`에서 차트용 데이터와 추천 도서 리스트를 받아옴 → View에 데이터 전달.

---
