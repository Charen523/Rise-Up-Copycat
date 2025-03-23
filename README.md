# 25 - 1 EXP 첫 번째 프로그래밍 과제

<table>
<tr>
<td>
<img src="https://github.com/user-attachments/assets/420003d7-b38a-4695-9d36-189547252013" width="280"/>
</td>
<td style="padding-left: 20px; vertical-align: top;">

<b>IN-GAME SCREENSHOT</b>  
<b>풍선을 지켜 오래 살아남으세요!</b>  
손가락(또는 마우스)과 함께 움직이는 보호막을 조작해 장애물을 막아보세요.  
풍선이 장애물에 닿는 순간 게임은 종료됩니다.

---

이 게임은 한때 어린 시절 큰 인기를 끌었지만, 지금은 점차 잊혀져 가는 <b>Rise Up!</b>의 모작입니다.  
UI가 단순하고 개발 난이도가 비교적 낮다고 판단하여, 일주일이라는 짧은 기간 안에 완성하기 좋은 주제로 선택하게 되었습니다.  

게임은 모바일 환경을 고려해 세로 화면으로 구성했으며, 실제 빌드는 Windows 플랫폼으로 진행했습니다.  
조작은 오직 터치 기반으로, 시작 버튼을 누르는 즉시 게임이 시작되고 맵이 아래로 스크롤되기 시작합니다.  

시간상의 제약으로 맵은 2종류만 구현되어 있으며, 이들은 랜덤으로 등장합니다.  
각 맵의 중간에는 잠시 쉴 수 있는 구간도 포함되어 있습니다.  
씬 전환 없이 맵 프리팹 자체가 이동하는 방식으로 제작되었으며,  
<b>일시정지</b>와 <b>게임 오버</b>는 <code>Time.timeScale = 0</code>을 활용해 구현하였습니다.  

UI는 평소에 사용하던 <code>UIManager</code> 구조를 그대로 적용했고,  
점수는 초당 1점씩 자동으로 증가합니다.  
풍선이 맵의 장애물에 닿는 순간 게임이 종료되며,  
플레이어는 손가락(또는 마우스 포인터)과 함께 움직이는 보호막을 통해 최대한 풍선을 오래 보호해야 합니다.  

⚠️ 게임을 반복해서 시작할 경우 간헐적으로 버그가 발생할 수 있으며,  
게임을 완전히 껐다가 다시 실행하면 최고 점수는 정상적으로 갱신됩니다.

</td>
</tr>
</table>
