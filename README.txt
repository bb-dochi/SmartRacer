/*SmartRacer 프로젝트 관련 파일설명입니다*/

*Android (Controler_test.zip)
-Main.java : 접속할 IP주소를 입력받는 곳
-Game.java : 게임에 접속 하여 센서값과 버튼 이벤트값을 서버로 전송

*Server - serverTest.java : 어플로 부터 데이터를 받아 게임으로 전송해줌

*Game - SmartRacer.zip
- AImove.cs : AI가 ItweenPath를 따라 이동하는 기능
- AIwheel.cs : AI 자동차 휠 회전 기능
- cameraFllow.cs : 카메라가 타겟을 따라가는 기능
- CarTrigger.cs : finish라인과 boost아이템 충돌 체크
- CntDownTrigger.cs : CountDown방식의 게임일 경우, 몇바퀴 돌았는지, 다 돌았는지 체크
- moveCtrl.cs : 서버로부터 받은 값을 활용해 플레이어 이동 컨트롤
- ScrollSnapRect.cs : choice씬에서 화면 스크롤 관련
- Skidmark.cs : 플레이어가 드리프트 시전 시 스키드 마크 표시
- SM.cs : 씬 움직임 관련 스크립트
- SoundManager.cs : 사운드 관련 스크립트
- TimerScript.cs : 게임 전체적인 시간관련 스크립트
- Trigger.cs : 누가 이겼는지 충돌 체크와 플레이어가 거꾸로 주행하지 못하게 함
- whellCtrl.cs : 플레이어 자동차 휠 회전 기능
- WhoisWinner.cs : 피니쉬 상태에 따른 UI변경 스크립트

*smartRacer_final_Data.exe 게임 실행파일 (똑같은 이름의 데이터 폴더와 같이 존재해야함)