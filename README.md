# (C# 코딩) 파일비교 프로그램

## 개요-C# 프로그래밍학습-1줄소개: 버튼을 클릭해 파일을 선택하면, 선택한 파일의 내용을 ListBox에 표시,복사,붙여넣기 기능을 제공하는 파일비교 프로그램입니다.
-사용한플랫폼: -C#, .NET Windows Forms, Visual Studio, GitHub
-사용한컨트롤:-Label, TextBox,SplitContainer, Button,Panel,ListView
-사용한기술과구현한기능:
-Visual Studio를이용하여UI 디자인
-Dock 기능을 이용하여 패널 위치 이동
-Anchor 기능을 이용하여 컨트롤 크기 조절


## 실행화면(과제1)
-코드의실행스크린샷과구현내용설명

![실행화면](img/screen1.png)


-구현한내용(위그림참조)
-UI 구성: Label(앱이름표시), TextBox2개(파일 경로), Button(파일 선택), Panel(버튼 위치), ListView(파일 내용 표시)
-속성 : Dock, Anchor를 이용하여 컨트롤 위치와 크기 조절
-이벤트핸들러: Button 클릭 이벤트를 통해 파일 선택 대화상자를 열고, 선택한 파일의 경로를 TextBox에 표시
-컨트롤에서 기본적으로 제공하는 기능 구동 확인(파일 선택, 내용 표시)