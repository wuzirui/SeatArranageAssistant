   M S B A M L   `   `   `  ����O  KSeatArrangeAssistant, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:SeatArrangeAssistantSeatArrangeAssistant  5   	   %    SeatArrangeAssistant.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:SeatArrangeAssistant     5   	     � Title$  
MainWindow��5   	   $��654.183��6   $��1212.785��6-   .��5	      � .+�5
   
   � -   5         �Name$ Board��5
      $��10,10,321,10q�6   #��� ��64   �� 5   
   $��ToolBox��6   $��Right=�6$   $	��540��6@   $��	0,10,10,0q�6M   $	��Top=�6`   $	��292��6x   .��5      � .+�5      �� -   5         ��Name$
 init��5      $��
Initialize��6'   $
��Left=�6<   $��
118,11,0,0q�6W   $	��Top=�6k   $��65��6�   $��24��6�    J� RenderTransformOrigin$ -0.321,0.375&�6�   �� 5      -   5         ��Name$ RowBox��5      $
��Left=�6*   $��23��6E   $��	64,11,0,0q�6Q    �� TextWrapping$
 Wrap=�6d   $��7��6x   $	��Top=�6�   $��49��6�    � ToolTip$ 
Row Number��6�   $ -0.437,1.696&�6�   �� 5      -   5        $ ColBox��5      $
��Left=�6*   $��23��6E   $��	10,10,0,0q�6Q   $
 Wrap=�6d   $��6��6x   $	��Top=�6�   $��49��6�   $ Colum Number��6�   �� 5      $��设置空位��6   $
��Left=�6(   $��
188,11,0,0q�6C   $	��Top=�6W   $��82��6o   $��23��6z   {�.  6�   �� 5      -   5        $��对调位置��5      $
��Left=�6(   $��	10,50,0,0q�6C   $	��Top=�6V   $	��103��6n   6z   �� 5      -   5         ��Name$ effect��5      $��保留特效��6+   $
��Left=�6:   $��
197,53,0,0q�6U   $	��Top=�6i    d� 	IsChecked$
 TrueY�6�   �� 5      -   5        	 ��Name$	 Speed��5      $
��Left=�6(   $��
118,50,0,0q�6C   $	��Top=�6W   $��74��6o   
 �� AutoToolTipPlacement$
 TopLeft=�6z   $ Speed��6�    � Maximum$
 1000Q�6�    � Minimum$ 50Q�6�    � Value$	 200Q�6�   �� 5      -   5        $
 save��5      $��保存��6'   $
��Left=�64   $��	10,90,0,0q�6O   $	��Top=�6b   $��98��6z   $��20��6�   6�   �� 5      -	   5        $��	另存为��5      $
��Left=�6'   $��
118,91,0,0q�6B   $	��Top=�6V   $��65��6n   6y   5      5      �� 5   
   -
   5        $��Generate��5      $��Right=�6$   $��	0,0,10,10q�6@   $	��292��6S   $��58��6_   $��Bottom=�6k   6�   5      5      