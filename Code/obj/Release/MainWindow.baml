   M S B A M L   `   `   `  ���� O  KSeatArrangeAssistant, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:SeatArrangeAssistantSeatArrangeAssistant  %    SeatArrangeAssistant.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     4local"clr-namespace:SeatArrangeAssistant       � Title$(  "SeatArrangeAssistant Version 1.4.1��$��390.183��$��1212.785��.��� .+�� -    �Name$ Board��$��10,10,295,10q�#��� ���� $��ToolBox��$��Right=�$	��280��$��	0,10,10,0q�$	��Top=�$	��280��.��� .+��� -    ��Name$
 init��$��
Initialize��$
��Left=�$��
118,11,0,0q�$	��Top=�$��65��$��24�� J� RenderTransformOrigin$ -0.321,0.375&��� -    ��Name$ RowBox��$
��Left=�$��23��$��	64,11,0,0q� �� TextWrapping$
 Wrap=�$��6��$	��Top=�$��49�� � ToolTip$ 
Row Number��$ -0.437,1.696&��� -   $ ColBox��$
��Left=�$��23��$��	10,10,0,0q�$
 Wrap=�$��6��$	��Top=�$��49��$ Colum Number���� $��设置空位��$
��Left=�$��
188,11,0,0q�$	��Top=�$��70��$��23��{�.  �� -   $��对调位置��$
��Left=�$��	10,50,0,0q�$	��Top=�$	��103���� -    ��Name$ effect��$��保留特效��$
��Left=�$��
192,53,0,0q�$	��Top=� d� 	IsChecked$
 TrueY��� -   	 ��Name$	 Speed��$
��Left=�$��
118,50,0,0q�$	��Top=�$��74��
 �� AutoToolTipPlacement$
 TopLeft=�$ Speed�� � Maximum$
 1000Q� � Minimum$ 50Q� � Value$	 200Q��� -   $
 save��$��保存��$
��Left=�$��	10,90,0,0q�$	��Top=�$��98��$��20���� -	   $��	另存为��$
��Left=�$��
118,91,0,0q�$	��Top=�$��65���� -
   $��Generate��$��Right=�$��	0,0,10,10q�$	��280��$��54��$��Bottom=�