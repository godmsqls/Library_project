## 팀 프로젝트 기술 문서

### 1. **폴더 구조 확인**

```
LibrarySystem/
│
├──   Controllers/                  #  [제어 계층] 시스템 전반의 흐름 및 요청 제어
│   │                                # - 규칙: 로그인/사서/유저/큐레이션 등 역할별 책임 분할
│   │                                # - 의존성: Views, ViewModels, Services (Models 직접 접근 금지)
│   ├── AuthController.cs            # 로그인/로그아웃 및 사서·유저 권한 제어
│   ├── LibrarianController.cs       # [사서 전용] 도서 등록(ISBN), 연체자 목록 관리
│   ├── UserController.cs            # [유저 전용] 도서 대출 및 반납(ISBN)
│   └── CurationController.cs        # [유저 전용] 분야별 통계 및 편식 방지 도서 추천
│
├──   Models/                       #  [데이터 계층] DB 테이블과 1:1 매핑되는 엔티티 (EF Core)
│   │                                # - 규칙: Getter/Setter 포함, 순수 데이터 구조만 유지
│   │                                # - 의존성: Services에서만 호출됨 (Controller와 격리)
│   ├── User.cs                      # 사용자 정보 (ID, PW, 권한, 연체 상태)
│   ├── Book.cs                      # 도서 정보 (ISBN, 국중도 청구기호, 분야명 등) - ?
│   └── LoanRecord.cs                # 대출 이력 (대출ID, UserID, ISBN, 대출/반납일) - ?
│                                       # User DB로만 전체 관리해도 괜찮지 않을까? - 토의해볼것.
│
├──   ViewModels/                   #  [데이터 전달 계층] View 렌더링을 위한 전처리 데이터
│   │                                # - 규칙: Getter/Setter 포함, Services의 데이터(JSON 등)를 UI 출력용(String)으로 파싱
│   │                                # - 의존성: Controllers (Controller가 생성하여 View로 전달)
│   ├── BookRegistrationVM.cs        # 사서의 도서 등록 폼 데이터 (유효성 검증 포함)
│   ├── LoanReturnVM.cs              # 유저의 대출/반납 입력 폼 데이터
│   ├── UserReadingStatsVM.cs        # 분야별 독서 기록 차트 데이터 (예: 문학 40%, 과학 10%)
│   └── AntiBiasRecommendVM.cs       # 편식 방지 추천 도서 리스트 데이터
│
├──   Views/                        #  [UI 계층] 사용자에게 보여지는 Windows Forms 화면
│   │                                # - 규칙: 비즈니스 로직 철저히 배제, UI 이벤트만 Controller로 전달
│   │                                # - 의존성: Controllers
│   ├──   Auth/                     # 로그인 폼 화면 (LoginForm.cs 등)
│   ├──   Librarian/                # 도서 등록 폼 및 연체자 관리 대시보드 화면
│   ├──   User/                     # 대출 및 반납 폼 화면
│   └──   Curation/                 # 분야별 독서 차트 및 추천 도서 표시 화면
│
└──   Services/                     #  [비즈니스 로직 계층] 핵심 처리 및 외부 API 연동
    │                                # - 규칙: Getter/Setter 포함, Models를 활용해 데이터 가공 및 DB 조작
    │                                # - 의존성: Models (DB 접근), Controllers (결과 반환)
    ├── AuthService.cs               # DB 기반 인증 및 권한 부여 로직
    ├── LibraryService.cs            # 도서 DB CRUD, 대출/반납 승인, 연체자 상태 업데이트 로직
    ├── NlkApiService.cs             # [국립중앙도서관 API] ISBN → 청구기호/분야 변환 통신
    ├── AladinApiService.cs          # [알라딘 API] 분야 코드 → 베스트셀러/신간 도서 검색 통신
    └── RecommendationService.cs     # 도서 추천 알고리즘 (유저 대출 이력 분석 및 교차 추천)
```
