
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// 어셈블리의 일반 정보는 다음 특성 집합을 통해 제어됩니다.
// 어셈블리와 관련된 정보를 수정하려면
// 이 특성 값을 변경하십시오.
[assembly: AssemblyTitle("EFEM")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("EFEM")]
[assembly: AssemblyCopyright("Copyright ©  2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// ComVisible을 false로 설정하면 이 어셈블리의 형식이 COM 구성 요소에 
// 표시되지 않습니다.  COM에서 이 어셈블리의 형식에 액세스하려면 
// 해당 형식에 대해 ComVisible 특성을 true로 설정하십시오.
[assembly: ComVisible(false)]

// 이 프로젝트가 COM에 노출되는 경우 다음 GUID는 typelib의 ID를 나타냅니다.
[assembly: Guid("d25530cf-6aca-46cf-8f66-73ce5adc0921")]

// 어셈블리의 버전 정보는 다음 네 가지 값으로 구성됩니다. 
//
//      주 버전
//      부 버전 
//      빌드 번호
//      수정 버전
//
// 모든 값을 지정하거나 아래와 같이 '*'를 사용하여 빌드 번호 및 수정 버전이 자동으로
// 지정되도록 할 수 있습니다.
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyFileVersion("5.18.46.94")]

// Version : [호환성.추가.변경.개선]
#region <VERSION HISTORY>

#region <5.18.46.94>
// 1. OHT 요청 시 LoadingMode에 따라 요청 랏 이름을 하드코딩 -> 파라메터 적용되도록 개선
#endregion </5.18.46.94>

#region <5.18.46.93>
// 1. Loadport Station 정보 얻어올 시, 사전에 값이 없는 경우 예외처리
// 2. AppConfig에서 LoadingMode 버그 개선
#endregion </5.18.46.93>

#region <5.18.46.92>
// 1. 마이그레이션 추가
// 2. JsonSubstrateLocationHistoryStorage의 ReadChangesAsync 메서드에서 파일이 없는경우 null 반환에 의해 프로그램 터지던 문제 개선 
#endregion </5.18.46.92>

#region <5.18.46.90>
// 1. ClosedCassette 추가 및 로봇 스테이션 분리
// 2. AppConfig LoadingMode 타입 추가
#endregion </5.18.46.90>

#region <5.18.45.90>
// 1. Process Start/End Event 누락되는 현상 부분 개선
#endregion </5.18.45.90>

#region <5.18.45.89>
// 1. TraceData에서 SupplyWaferId null이면 터지던 부분 개선
#endregion </5.18.45.89>

#region <5.18.45.88>
// 1. 마지막 자재 픽업 하지 않던 현상 2차 개선
#endregion </5.18.45.88>

#region <5.18.45.87>
// 1. 마지막 자재 픽업 하지 않던 현상 개선
#endregion </5.18.45.87>

#region <5.18.45.86>
// 1. MaterialTracking 저장소 개선 및 Json&Sqlite 저장소 추가
#endregion </5.18.45.86>

#region <5.17.45.86>
// 1. MaterialTracking 저장소 분리 및 SQLite 추가
#endregion </5.17.45.86>

#region <4.17.45.86>
// 1. E84 PIO TEST Fail 항목 수정
#endregion </4.17.45.86>

#region <4.17.45.85>
// 1. Substrate/Substrate 단일 책임원칙에 의한 역할 분리
//  1) Substrate
//      - 파일 읽기/저장기능 관련 Storage로 이관
//      - 기본 Attribute 속성화 및 추가 Attribute(Extra) 분리
//          -> 클래스를 인터페이스로 변경
//      - 프로세스 타입별 분리되는 부분은 ISubstrateProcessProfileFactory, IProcessTypeProvider 으로 이관
//          -> SubstrateManager의 Configure에서 결정되어 매 자재 생성 시 속성부여됨
//  2) SubsttateManager
//      - 파일 저장부 ISubstrateStorage로 이관
//      - ISubstrateProcessProfileFactory, IProcessTypeProvider를 받아서 팩토리 패턴에 의해 자재 속성 부여
//      - ISubstrateStorage는 Json 으로 변경(XML 대비 경량화) -> 추후 DB 사용 시 변경(혹은 혼합 사용)
//          -> JSon은 "Newtonsoft.Json" 사용(System.Text.JSon은 .net 버전 호환성 문제로 어셈블리 에러 발생)
// 2. SubstrateName 기반을 전부 UniqueKey로 교체 -> 아직 남아있는 부분은 추후 수정 예정
// 3. SubstrateManager에서 자재 정보 반환 객체 관련 인터페이스 축소 및 변경
#endregion </4.17.45.85>

#region <4.16.45.85>
// 1. Substrate Manager 대규모 수정
// 2. E84 PIO Interface 대규모 수정
#endregion </4.16.44.85>

#region <4.16.44.85>
// 1. Map Download Error 발생 시 Spilt 진행 안하도록 수정
// 2. Supply Wafer Map Angle 관련 하드코딩 제거
#endregion </4.16.44.85>

#region <4.16.44.84>
// 1. 500W 스케쥴러 수정 - Arm에 Empty Wafer가 있을 때, Core_12 Loading 요청이 오면 무언정지 현상 발생 수정
#endregion </4.16.44.84>

#region <4.16.44.83>
// 1. 500W 스케쥴러 대규모 수정
#endregion </4.16.44.83>

#region <4.16.44.82>
// 1. AutoRun 가동 중 500W 암에 로딩을 위한 자재 보유 시에 로딩이 아닌 언로딩 요청이 오면 로드포트에 자재 안착 후 요청 진행하도록 개선
#endregion </4.16.44.82>

#region <4.16.44.81>
// 1. Task Action Start/End/Alarm 로그 기록 추가
#endregion </4.16.44.81>

#region <4.16.44.80>
// 1. Manual Operation 에서 암에 자재가 모두 있을 시 잘못된 자재를 로딩하던 버그 개선
#endregion </4.16.44.80>

#region <4.16.44.79>
// 1. AtmRobot Action 단위 로그 추가
#endregion </4.16.44.79>

#region <4.16.44.78>
// 1. Core/Empty Carrier Unload 시 Lot Id Writing 조건 개선
#endregion </4.16.44.78>

#region <4.16.44.77>
// 1. LoadPort TransferPort Status 상태 전이 조건 개선
#endregion </4.16.44.77>

#region <4.16.44.76>
// 1. EFEM Module Log 개선
#endregion </4.16.44.76>

#region <4.16.43.76>
// 1. 500W TraceData 추가
#endregion </4.16.43.76>

#region <4.16.43.75>
// 1. Task부 생성자 시나리오 버그 개선
#endregion </4.16.43.75>

#region <4.16.43.73>
// 1. Pick/Place End 시 자재 정보 찾는 부분 개선
#endregion </4.16.43.73>

#region <4.16.43.72>
// 1. 라인 반출본 머지 & Unloading 시작부 Smema On 부분 로직 개선
#endregion </4.16.43.72>

#region <4.16.43.71>
// 1. 라인 테스트
#endregion </4.16.43.71>

#region <4.16.43.70>
// 1. 500W 통신 추가
#endregion </4.16.43.70>

#region <4.16.42.70>
// 1. Gem 미 사용 시 맵 다운로드 가능하도록 개선
#endregion </4.16.42.70>

#region <4.16.42.69>
// 1. 가치효율 추가
#endregion </4.16.42.69>

#region <4.16.41.69>
// 1. Base프로그램 병합(Base Project_2025.06.24_V1)
#endregion </4.16.41.69>

#region <3.16.41.69>
// 1. 8인치 wcf 복구
#endregion </3.16.41.69>

#region <3.16.41.68>
// 1. UniqueKey 추가 및 파일이름기준 : Name -> UniqueKey로 변경하여 이름이 같은 경우도 파일 중복 및 의도되지 않은 정보누락 개선
#endregion </3.16.41.68>

#region <3.16.40.68>
// 1. 공정설비에서 자재 언로딩 중 자재 정보를 찾는 부분 수정
#endregion </3.16.40.68>

#region <3.16.40.67>
// 1. 물류(Unloading) 현장맞춤 수정(500W)
#endregion </3.16.40.67>

#region <3.16.40.66>
// 1. 배포를 위한 버전업
#endregion </3.16.40.66>

#region <3.16.40.65>
// 1. 코어 캐리어 머지 진행 여부 수정(모두 랏 이름이 같으면 진행 x)
// 2. 1d 테스트 이후 반출코드 머지
#endregion </3.16.40.65>

#region <3.16.40.62>
// 1. Initializer에서 Digital IO IsTransitionComplete 메서드 인자 개선(Output: False여야 하는 것을 Input으로 함)
#endregion </3.16.40.62>

#region <3.16.40.61>
// 1. 가치효율, SlotMapping 이벤트 svid 추가
#endregion </3.16.40.61>

#region <3.16.39.61>
// 1. Terminal Log 분리
// 2. WCF 연결 실패 시 Exception 안 남도록 추가
#endregion </3.16.39.61>

#region <3.16.38.61>
// 1. Trigger 변경에 따른 코드 개선
#endregion </3.16.38.61>

#region <3.16.37.61>
// 1. 로드포트 초기화 중 캐리어 없을 시 자재 정보 제거하도록 개선
#endregion </3.16.37.61>

#region <3.16.37.60>
// 1. PartId 비어있을 때 버그 개선
#endregion </3.16.37.60>

#region <3.16.37.59>
// 1. PartId 정보 가져오지 않던 부분 개선
#endregion </3.16.37.59>

#region <3.16.37.58>
// 1. 1D 추가
#endregion </3.16.37.58>

#region <3.16.36.58>
// 1. 조건 변경 및 버그 수정
#endregion </3.16.36.58>

#region <3.16.36.57>
// 1. ' 제거
#endregion </3.16.36.57>

#region <3.16.36.56>
// 1. Merge 시 모랏자재 터미네이트될 때 조건 버그 개선
#endregion </3.16.36.56>

#region <3.16.36.55>
// 1. AMHS 버그 개선
#endregion </3.16.36.55>

#region <3.16.36.54>
// 1. FrameWork Update
#endregion </3.16.36.54>

#region <2.16.36.54>
// 1. Unload 시 Lower Arm만 사용하던 부분 개선
#endregion </2.16.36.54>

#region <2.16.36.53>
// 1. Robot Arm 하나 Off 시 버그 수정
#endregion </2.16.36.53>

#region <2.16.36.52>
// 1. 예외추가
#endregion </2.16.36.52>

#region <2.16.36.51>
// 1. History Summary 추가
#endregion </2.16.36.51>

#region <2.15.36.51>
// 1. 공정설비 자재정보 제거되지 않던 버그 개선
#endregion </2.15.36.51>

#region <2.15.36.50>
// 1. Lot History UI 추가
#endregion </2.15.36.50>

#region <2.14.36.50>
// 1. FDC ESD 누락 개선
#endregion </2.14.36.50>

#region <2.14.36.49>
// 1. Substrate Attribute 추가함
#endregion </2.14.36.49>

#region <2.14.35.49>
// 1. Substrate History Carrier에 같이 기록되도록 변경
#endregion </2.14.35.49>

#region <2.14.34.49>
// 1. Substrate List Concurrent로 변경하여 컬렉션 변경 에러 개선
// 2. EFEM에서 레시피 업로드 시 파일 없어도 현재 레시피 복사 후 올리도록 개선
#endregion </2.14.34.49>

#region <2.14.34.48>
// 1. SecsMessage 버그 개선
#endregion </2.14.34.48>

#region <2.14.34.47>
// 1. 예외추가
#endregion </2.14.34.47>

#region <2.14.34.46>
// 1. Ring Id를 이용해 Substrate를 찾을 때 Attribute를 이용해 찾도록 개선
#endregion </2.14.34.46>

#region <2.14.33.46>
// 1. RobotActionScheduler의 Hascarrier Empty Tape Loading 조건 개선
#endregion </2.14.33.46>

#region <2.14.33.45>
// 1. 500BIN LotHistory Backup Path 변경
#endregion </2.14.33.45>

#region <2.14.33.44>
// 1. 500BIN LotHistory 추가
#endregion </2.14.33.44>

#region <2.13.33.44>
// 1. 500BIN TraceDataRecovery 추가
// 2. 500BIN Customized에 RecipeHandling 추가
#endregion </2.13.33.44>

#region <2.12.33.44>
// 1. 500W 작업 시작
#endregion </2.12.33.44>

#region <2.11.33.44>
// 1. StartSorting, FinishSorting 시 Carrier 정보 보던 부분 개선
#endregion </2.11.33.44>

#region <2.11.33.43>
// 1. 공정설비와 통신 도중 스킵되는 경우, 로봇 스케쥴링을 다시 태우기 위해 포트 설정 위치 변경
// 2. Substrate 이름 룰 변경(고유하게 하도록)
#endregion </2.11.33.43>

#region <2.11.32.43>
// 1. 버그수정, 구버전 변화부분 업데이트 필요
#endregion </2.11.32.43>

#region <2.11.32.42>
// 1. BinDataUpload 자동 발생 하던 부분을 AtmRobot에서 발생하도록 변경(에러 발생 시 인터락을 위함)
// 2. UploadBinData 시나리오 생성
#endregion </2.11.32.42>

#region <2.11.31.42>
// 1. Material Edit 기능 적용 안 되던 부분 개선
// 2. InAccess 상태부터 작업하도록 개선
#endregion </2.11.31.42>

#region <2.11.31.41>
// 1. 현장 테스트 버전
#endregion </2.11.31.41>

#region <2.11.31.40>
// 1. 캐리어 로드 이후 매뉴얼로 바로 OHT Unloading Call 시 Lot, Carrier Id 반영되지 않아 읽고 보내도록 변경
#endregion </2.11.31.40>

#region <2.11.30.40>
// 1. Substrate Recovery Data Load/Save 시 null check 추가
#endregion </2.11.30.40>

#region <2.11.30.39>
// 1. Robot Pick/Place 시 시나리오를 Queue에 쌓고 실행하도록 개선 -> 추후 단위 동작 완료 Flag(Recovery)를 통해 이어서 작업하도록 구현하기 수월하도록 개선함
#endregion </2.11.30.39>

#region <2.11.29.39>
// 1. Robot Recovery Update 메서드 추가
// 2. Main UI 위치 벗어나던 부분 개선
// 3. ManulOperationLoadPort에 CarrierAccessStatus 변경하도록 추가
#endregion </2.11.29.39>

#region <2.11.28.39>
// 1. 일부 namespace 정리 및 폴더, 파일 구조 변경
// 2. 모듈 Retry 추가. 3회 시도 후 시도하지 않도록 변경
// 3. 태스크 이니셜&엔트리에서 각 모듈 연결상태 확인 및 알람 추가
#endregion </2.11.28.39>

#region <2.11.27.39>
// 1. UI Table Layout 추가하여 확장/축소되게 수정(임대웅)
#endregion </2.11.27.39>

#region <2.11.26.39>
// 1. Detaching 시 데이터 정렬 후 보내도록 변경
#endregion </2.11.26.39>

#region <2.11.26.38>
// 1. 포트별 OHT 호출 시나리오 분리
#endregion </2.11.26.38>

#region <2.11.25.38>
// 1. 고객사 요청에 맞게 공테이프 완료 시나리오 변경
#endregion </2.11.25.38>

#region <2.11.24.38>
// 1. Xedion RFID 읽고 쓰기 커맨드 전송 시 버그 개선
#endregion </2.11.24.38>

#region <2.11.24.37>
// 1. SecsGem Logger 구조 변경
#endregion </2.11.24.37>

#region <2.11.23.37>
// 1. 레시피 이름 전송 시 공정설비 것으로 갱신하도록 변경
#endregion </2.11.23.37>

#region <2.11.22.37>
// 1. 프로세스 스타트 버그 개선
#endregion </2.11.22.37>

#region <2.11.22.36>
// 1. RFID 수정
#endregion </2.11.22.36>

#region <2.11.22.35>
// 1. FDC Dictionary -> ConcurrentDictionary로 변경(크로스 스레드)
#endregion </2.11.22.35>

#region <2.11.22.34>
// 1. ECID 다른 값만 갱신하도록 변경
#endregion </2.11.22.34>

#region <2.11.21.34>
// 1. SELOP, NRC 코드 개선
#endregion </2.11.21.34>

#region <2.11.21.33>
// 1. PMS 파일 생성 시 Slot + 1 하도록 개선(서버는 1부터 시작)
#endregion </2.11.21.33>

#region <2.11.21.32>
// 1. Quick Button 이벤트 누락 개선
#endregion </2.11.21.32>

#region <2.11.21.30>
// 1. Quick Button 이벤트 누락 개선
#endregion </2.11.21.30>

#region <2.11.21.30>
// 1. Quick Button 추가
#endregion </2.11.21.30>

#region <2.10.21.30>
// 1. Trackout 사용유무 욥션 적용
#endregion </2.10.21.30>

#region <2.10.21.29>
// 1. UploadBinFile Permission 갱신 개선
#endregion </2.10.21.29>

#region <2.10.21.28>
// 1. 마지막 자재 판단로직개선
#endregion </2.10.21.28>

#region <2.10.21.27>
// 1. 통신 관련 버그 수정
#endregion </2.10.21.27>

#region <2.10.21.26>
// 1. MergeAngChange 버그 수정
#endregion </2.10.21.26>

#region <2.10.21.25>
// 1. 통신사양 변경으로 인한 대응 버전
#endregion </2.10.21.25>

#region <2.10.20.25>
// 1. Client Message ECID, TraceData 기능 변경
#endregion </2.10.20.25>

#region <2.10.19.25>
// 1. 글로벌 태스크 엔트리에서 로봇 태스크 매뉴얼 액션 동작 시만 공정설비와의 통신 확인하도록 개선
#endregion </2.10.19.25>

#region <2.10.19.24>
// 1. 임대웅 대리 코드와 병합(SELOP8Controller, NRCRobotController, AtmRobotOperator 구분 동작)
#endregion </2.10.19.24>

#region <2.9.19.24>
// 1. 시나리오 버그 수정
#endregion </2.9.19.24>

#region <2.9.19.23>
// 1. 시나리오 버그 수정
#endregion </2.9.19.23>

#region <2.9.19.21>
// 1. Customized에서 레시피 분리 중
#endregion </2.9.19.21>

#region <2.9.18.21>
// 1. Recipe Handling 변경
#endregion </2.9.18.21>

#region <2.9.17.21>
// 1. SecsGem Customized에 시나리오 실행 부분을 버그 수정
#endregion </2.9.17.21>

#region <2.9.17.20>
// 1. SecsGem Customized에 시나리오 실행 부분을 Circulator -> ExecuteScenarioAsync로 변경
// 2. 실행 후 후속 Setting 관련 추가
#endregion </2.9.17.20>

#region <2.8.17.20>
// 1. SecsGem Customized UI 기능 추가
#endregion </2.8.17.20>

#region <2.7.17.20>
// 1. 로봇 암의 자재 편집 기능 추가
// 2. 캐리어맵매니저 자원 소모 개선
#endregion </2.7.17.20>

#region <2.6.17.20>
// 1. 캐리어맵에 나가있는 자재도 표시되도록 변경
#endregion </2.6.17.20>

#region <2.6.16.20>
// 1. ManualOperation Loading/Unloading UI 제거
// 2. Recipe의 LoadPortType Enum 변경(사용자 요청)
#endregion </2.6.16.20>

#region <2.6.15.20>
// 1. AtmRobot Arm 사용유무 추가
#endregion </2.6.15.20>

#region <2.5.15.20>
// 1. AtmRobot 물류 시 알람 코드 세분화
#endregion </2.5.15.20>

#region <2.5.14.20>
// 1. WORK_END Data 버그 수정
#endregion </2.5.14.20>

#region <2.5.14.19>
// 1. Base Framework 적용(2024.06.18버전)
#endregion </2.5.14.19>

#region <1.5.14.19>
// 1. LoadPort 미사용 시에도 홈 진행하며, 도킹 시에는 언도킹되도록 개선
#endregion </1.5.14.19>

#region <1.5.14.18>
// 1. LoadPort에 자재 정보 존재 시 Place 못하게 인터락 추가
// 2. 로봇 스케쥴러의 공정설비와 인터페이스 부분 TaskAtmRobotPWA500Bin으로 이동
// 3. TransferBlocked -> ReadyToUnload 전이 조건에 CarrierAccessingStatus 보도록 변경
#endregion </1.5.14.18>

#region <1.5.13.18>
// 1. CrevisModbus용 Socket Class 변경
#endregion </1.5.13.18>

#region <1.5.12.18>
// 1. 버그수정
#endregion </1.5.12.18>

#region <1.5.12.17>
// 1. QuickButtons에 있던 Digital Input Monitoring 을 Monitoring 의 Analog/Digital 분리 및 이동
#endregion </1.5.12.17>

#region <1.5.11.17>
// 1. UseSecsGemWithCoreWaferMapHandlingOnly 옵션 추가
// 2. CarrierMap에 Substrate 이름 출력하도록 추가
#endregion </1.5.11.17>

#region <1.5.10.17>
// 1. Modbus IO 신규 클래스 추가(버그 개선용 객체 변경)
// 2. WCF 서비스 재연결 부분 변경(클라이언트 및 섹젬도 수정 필요?)
#endregion </1.5.10.17>

#region <1.5.10.16>
// 1. TaskGlobal Door/Utility 발생 안 하던 버그 개선
// 2. 파일 백업 임시 막음
#endregion </1.5.10.16>

#region <1.5.10.15>
// 1. SecsGem EventList 구현
#endregion </1.5.10.15>

#region <1.5.9.15>
// 1. ScenarioSendEventThenHandlingSecsMessage Permission 개선
// 2. Cancel Carrier 구현
#endregion </1.5.9.15>

#region <1.5.8.15>
// 1. Lot Info 정보 갱신 부분 수정
#endregion </1.5.8.15>

#region <1.5.8.14>
// 1. LotInfo 응답 수정
#endregion </1.5.8.14>

#region <1.5.8.13>
// 1. MapDownload 관련 버그 수정
#endregion </1.5.8.13>

#region <1.5.8.12>
// 1. 현장 검증 중 버전 업
#endregion </1.5.8.12>

#region <1.5.8.11>
// 1. VID 추가, Model Name, Version Update 추가
#endregion </1.5.8.11>

#region <1.5.8.10>
// 1. 시나리오 순서에 따른 변경. 통신 검증 전 버전업
#endregion </1.5.8.10>

#region <1.5.7.10>
// 1. 통신 1차 완료
#endregion </1.5.7.10>

#region <1.5.6.10>
// 1. 통신 내부 메시지 테스트 디버깅 중 수정
#endregion </1.5.6.10>

#region <1.5.6.9>
// 1. PIO 통신 시 정지 기능과 완료 타임아웃 변경
#endregion </1.5.6.9>

#region <1.5.6.8>
// 1. 통신 관련 객체 내부 인터페이스 추가
#endregion </1.5.6.8>

#region <1.5.5.8>
// 1. Robot/LoadPort 명령 보내기 전 결과 사전 초기화하도록 개선
#endregion </1.5.5.8>

#region <1.5.5.7>
// 1. TaskLoadPort/TaskAtmRobot Initialize 시 Busy, Clear 추가
#endregion </1.5.5.7>

#region <1.5.4.7>
// 1. Cassette Slot 1에 자재 있을 시 언로딩 후 에러 출력하도록 수정
#endregion </1.5.4.7>

#region <1.5.3.7>
// 1. E23 시퀀스 스텝 변경 시 버그 개선
#endregion </1.5.3.7>

#region <1.5.3.6>
// 1. Initialize 시 어떤 태스크를 선택하던 AtmRobot은 무조건 잡도록 변경
// 2. TaskLoadPort Initialize 시 AtmRobot 의 Home 완료 여부만 보던 것을 TaskAtmRobot Initialize 완료 후 Signal을 통해 진행하도록 변경
#endregion </1.5.3.6>

#region <1.5.2.6>
// 1. DuraportController Firmware 수정에 따른 TaskLoadPort Initializing 순서 변경
// 2. TaskLoadPort에서 Controller Alarm이 Event 형식 발생하는 경우에 따라 에러 출력 추가 -> 자동 Reset 시도(Async)
#endregion </1.5.2.6>

#region <1.5.2.5>
// 1. DuraportController에서 로드/언로드 시 완료 안 되던 문제 개선
// 2. StateTransitioner에서 Initialize 중 전이하지 않도록 개선
#endregion </1.5.2.5>

#region <1.5.2.4>
// 1. DuraportController에서 안착 관련 버그 개선
// 2. TaskLoadPort에서 초기화 중에는 Initialized 신호를 안 보도록 개선
#endregion </1.5.2.4>

#region <1.5.2.3>
// 1. DuraportController에서 Status 상시 받을 시 명령 완료 갱신(Idle)하던 버그 개선
#endregion </1.5.2.3>

#region <1.5.2.2>
// 1. TaskLoadPort 및 LoadPort 관련 객체(Manager, Operator 등) AMHS PIO Interface 기능 추가
#endregion </1.5.2.2>

#region <1.4.2.2>
// 1. TaskLoadPort Manual Action 추가
#endregion </1.4.2.2>

#region <1.3.2.2>
// 1. Carrier 이탈 알람 추가
// 2. Placement Error 추가
#endregion </1.3.2.2>

#region <1.2.2.2>
// 1. 공정설비 연결 상태 Main UI에 표시하도록 추가
#endregion </1.2.2.2>

#region <1.1.2.2>
// 1. Carrier Completed 조건 변경
#endregion </1.1.2.2>

#region <1.1.2.1>
// 1. 공정설비와 물류 시 자재정보 없는 경우 일단 픽업 후 에러 출력하도록 변경 -> 버그 픽스
#endregion </1.1.2.0>

#region <1.1.2.0>
// 1. RobotState에서 자재 정보 제거하도록 변경
// 2. 공정설비와 물류 시 자재정보 없는 경우 일단 픽업 후 에러 출력하도록 변경
#endregion </1.1.2.0>

#region <1.1.1.0>
// 1. LoadPort Mode 변환 로직 변경
#endregion </1.1.1.0>

#region <1.1.0.0>
// 1. SecsGem 객체 추가 및 Config 작성
#endregion </1.1.0.0>

#region <1.0.0.0>
// 최초 배포
#endregion

#endregion </VERSION HISTORY>