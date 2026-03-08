# Tasks
- C# Blazor 기능 및 UI 구현
- UI Layout은 AdminWebSite.pptx 참고

## 프로젝트 정의
### SmugglerSelectWeb
- 사이트 초기화면
- 로그인/회원가입 페이지 (ppt의 '1-로그인 화면' UI)
- 운영 리전 선택 화면 (ppt의 '2-리전(영역) 선택' UI)
- 로그인한 유저의 권한에 따른 선택가능한 리전 선택화면
- 리전별로 하위 사이트 접속시 추가적으로 유저의 권한이 부여된 상태로 하위 사이트로 접속 유지

### SmugglerWebCommon
- 블레이저용 공용 라이브러리/기능 모음 

### SmugglerWebTool
- ppt의 '3-사이트(Base Layout)' UI 부터 적용
- 메인 콘텐츠 기능 웹사이트
- SmugglerSelectWeb에서 받은 로그인 정보의 유지(API)
- 로그인 유지: 30분(추후 변경 가능)
- 서브 메뉴 구성하고 권한별 출력 여부 설정 적용
- 언어 변경 기능: 한국어(기본), 영어, 언어1, 언어2, ...
- 테마 변경 기능: Light/Black/System(Auto)
- 초기 화면은 기본으로 설정될 대쉬보드

## Tasks List

### SmugglerWebCommon
- EF Core 의존성 제거 및 데이터 접근 계층 Dapper 표준화
- 공통 DB 연결 팩토리/트랜잭션 유틸리티 정의 (Dapper 기준)
- 공통 인증/권한 모델 정의: User, Role, Region, Permission DTO 및 Enum 정리
- 공통 응답/오류 모델 정의: API 결과 래퍼, 페이징, Validation/Error 코드 규약
- 공통 인증 상태 관리 서비스 정의: 로그인 사용자/토큰/만료시간 저장 인터페이스
- 지역/권한 체크 유틸리티 작성: 리전 접근 가능 여부, 메뉴 노출 가능 여부 판별 함수
- 다국어 리소스 구조 정의: 기본(ko), en, language1, language2 리소스 파일/키 네이밍 규칙
- 공통 테마 설정 모델 정의: Light/Black/System 설정 값 및 저장 키 규칙

### SmugglerSelectWeb
- 초기 진입 페이지 구성 및 라우팅 연결
- 로그인 화면 UI 구현 (ppt 1-로그인 화면 기준)
- 회원가입 화면 UI 및 기본 검증(이메일 포함 필수값/형식) 구현
- 회원가입 API 스펙 확장: 이메일 필드 저장/중복 검사/검증 처리
- 비밀번호 RSA 암호화 처리: 클라이언트 암호화 + 서버 복호화 검증 플로우 적용
- 인증 API 연동: 로그인/회원가입 요청 및 결과 처리
- 로그인 성공 시 사용자 권한 목록/리전 목록 조회 API 연동
- 리전 선택 화면 UI 구현 (ppt 2-리전 선택 화면 기준)
- 사용자 권한 기반 리전 활성/비활성 처리
- 리전 선택 완료 시 SmugglerWebTool 진입 URL 생성(선택 리전/권한 컨텍스트 포함)
- 하위 사이트 전달용 인증 컨텍스트 저장(쿠키 또는 토큰 기반) 및 만료 정책 연결
- 비로그인/권한없음/세션만료 예외 화면 처리

### SmugglerWebTool
- 사이트 Base Layout UI 구현 (ppt 3-사이트 Base Layout 기준)
- 공통 헤더/사이드바/콘텐츠 영역 컴포넌트 분리
- SmugglerSelectWeb 전달 인증 정보 검증 API 연동
- EF Core 기반 인증/사용자 조회 로직 제거 후 Dapper 쿼리로 전환
- 로그인 세션 유지 30분 적용(자동 갱신 정책 포함, 설정값 분리)
- 권한 기반 서브 메뉴 구성 및 메뉴 노출 제어
- 초기 대시보드 페이지 구현(기본 진입 화면 설정)
- 언어 전환 기능 구현(ko 기본, en, language1, language2)
- 테마 전환 기능 구현(Light/Black/System) 및 사용자 설정 저장
- 리전/권한 컨텍스트가 반영된 공통 API 호출 파이프라인 구성
- 인증 실패/권한없음/서버오류 공통 처리(리다이렉트/알림 정책)
- 로그아웃 및 컨텍스트 초기화 처리

## 권장 구현 순서
1. SmugglerWebCommon 공통 모델/서비스 계약 정의
2. SmugglerSelectWeb 인증 + 리전 선택 플로우 완성
3. SmugglerWebTool Base Layout + 인증 연동
4. SmugglerWebTool 권한 메뉴/대시보드/다국어/테마 적용
