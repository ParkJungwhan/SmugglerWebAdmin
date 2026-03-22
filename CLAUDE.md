# SmugglerWebAdmin

## 프로젝트 개요
ASP.NET Core 10.0 기반 Blazor 풀스택 관리자 웹 애플리케이션.

## 솔루션 구조
```
SmugglerWebAdmin.slnx
Src/
  SmugglerWebAdmin/          # 서버 프로젝트 (ASP.NET Core 호스팅)
  SmugglerWebAdmin.Client/   # 클라이언트 프로젝트 (Blazor WebAssembly)
```

## 기술 스택
- .NET 10.0 / ASP.NET Core 10.0
- Blazor (Server + WebAssembly 하이브리드)
- Entity Framework Core 10.0.3 + SQL Server (LocalDB)
- ASP.NET Core Identity

## 빌드
```bash
"C:/Program Files/Microsoft Visual Studio/18/Community/MSBuild/Current/Bin/MSBuild.exe" SmugglerWebAdmin.slnx
```

## 실행
VS2026에서 `https` 프로파일로 F5 실행.
- URL: https://localhost:7102

## 데이터베이스
- SQL Server LocalDB
- 연결 문자열: `Src/SmugglerWebAdmin/appsettings.json` → `DefaultConnection`
- 마이그레이션 적용:
  ```bash
  cd Src/SmugglerWebAdmin
  dotnet ef database update
  ```

## 코드 특이사항
- `ImplicitUsings`: 두 프로젝트 모두 `enable` 상태 (전역 규칙 예외 — 기존 코드 유지)
- `Nullable`: 두 프로젝트 모두 `enable`
