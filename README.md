# SmugglerWebAdmin

- Admin용 Blazor Web Site 구현
- 사내 어드민 웹 사이트를 만드는 용으로 개발 예정
- C#(.Net 10), Blazor, Dapper, Postgresql

## 프로젝트 구분
### SmugglerSelectWeb
- 접속시 처음 출력되는 사이트로
- 로그인(+가입) 기능
- 리전이나 구분된 운영 부분을 선택하는 화면으로 출력

### SmugglerWebTool(+.Client)
- 메인 컨텐츠가 출력되는 전체 기능 사이트

### SmugglerWebCommon
- 공통 모듈 모음
