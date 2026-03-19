## 팀 프로젝트 기술 문서

### 2. **커밋 규정**

- 커밋 전 커밋 메시지 규정 확인하기
- `https://velog.io/@shin6403/Git-git-%EC%BB%A4%EB%B0%8B-%EC%BB%A8%EB%B2%A4%EC%85%98-%EC%84%A4%EC%A0%95%ED%95%98%EA%B8%B0 블로그 참조
- 프로젝트 업데이트 사항 확인 후 커밋하기

### 3. **폴더 구조 확인**

```
LibrarySystem/
│
├── Controllers/                 # HTTP 요청 처리 및 응답 흐름 제어
│   ├── AuthController.cs        # 로그인, 로그아웃 및 사서/유저 권한 제어
│   ├── LibrarianController.cs   # [사서 전용] ISBN 도서 등록, 연체자 목록 관리
│   ├── UserController.cs        # [유저 전용] ISBN 기반 도서 대출 및 반납
│   └── CurationController.cs    # [유저 전용] 분야별 통계 및 편식 방지 추천 도서 제공
│
├── Models/                      # 데이터베이스 엔티티 (Entity Framework Core 사용?)
│   ├── User.cs                  # 사용자 정보 (ID, PW, 권한, 연체 상태)
│   ├── Book.cs                  # 도서 정보 (ISBN, 국중도 청구기호, 분야명 등)
│   └── LoanRecord.cs            # 대출 이력 (대출ID, UserID, ISBN, 대출/반납일)
│
├── ViewModels/                  # View 렌더링을 위해 가공된 데이터 모델
│   ├── BookRegistrationVM.cs    # 사서 도서 등록 폼 데이터
│   ├── LoanReturnVM.cs          # 유저 대출/반납 입력 폼 데이터
│   ├── UserReadingStatsVM.cs    # 분야별 독서 기록 차트 데이터 (예: 문학 40%, 과학 10%)
│   └── AntiBiasRecommendVM.cs   # 편식 방지 추천 도서 리스트 데이터
│
├── Views/                       # 사용자 인터페이스
│   ├── Auth/                    # 로그인 화면
│   ├── Librarian/               # 도서 등록 폼 및 연체자 관리 대시보드
│   ├── User/                    # 대출 및 반납 폼 화면
│   └── Curation/                # 분야별 독서 차트 및 추천 도서 표시 화면
│
└── Services/                    # 핵심 비즈니스 로직 및 외부 API 연동
    ├── AuthService.cs           # 인증 및 권한 부여 로직
    ├── LibraryService.cs        # 도서 DB 관리, 대출/반납 승인, 연체자 상태 업데이트
    ├── NlkApiService.cs         # [국립중앙도서관 API] ISBN → 청구기호/분야 변환
    ├── AladinApiService.cs      # [알라딘 API] 분야 코드 → 베스트셀러/신간 도서 검색
    └── RecommendationService.cs # 편식 방지 추천 알고리즘 (유저 대출 이력 분석)

```
