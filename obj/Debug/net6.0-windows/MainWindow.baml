   M S B A M L   `   `   `  ����  SpotlightClone    SpotlightClone.MainWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  	Spotlight��5   	   $	��220��6   $	��600��6(    � WindowStyle$
 None=�5   	    � AllowsTransparency$
 True��5   	   ���	#DD1E1E1E
������� � Topmost$
 True��5	   	    � 
ResizeMode$ NoResize=�5
   	    � ShowInTaskbar$ False��5   	   .��5      ��  �� CornerRadius$ 20��6   ���	#FF2B2B2B$��10q�66    �� Padding$ 10q�6B      ShadowEffect J� Effect# �   5      ._�5   
   �� .+�5      �� -   5        	 ��Name$	 	SearchBox��5      $��24(�5      $��50��5      ���	#FF3A3A3A
�������$��0q�5      $��0,0,0,10q�5      $
��10,0q�5      $��Center=�5      $
��Left=�5      5      �� 5      -   5        
 ��Name$
 SuggestionList��5      ���	#FF2B2B2B$��0q�5       
�������$��18(�5"      $��5q�5#      $��0,0,0,0q�5$      $	��150��5%      $y�	Collapsed=�5&      $
��Auto=�5(      ��5)      �� 5*       �� 
TargetType" ��6   .�5+      ��  �� Property � �� �� Value$ 4q�63   �� 5,       � ��$ 8q�64   �� 5-       � ��
 ���� 67   �� 5.       �@�Cursor$
 Hand��63   �� 5/       � �� 50      �� 51   "    �� 
TargetType" ��62   .R�52   &   ��  ��Name$ Bd��6-   #��z��69   $ 6��53   -   ._�54   *   �� $
��Left=�6;   $��Center=�6V   $��4q�6q   55   '    �� Triggers 56   &   P� 57   *    P� Property �@J�IsMouseOver P� Value$
 True��6I   . �58   .   ��  �� 
TargetName$ Bd��65    � �� �#4446[   59   +   P� 5:   *    �@��
IsSelected$
 True��6H   . �5;   .   �� $ Bd��65    � �� �#6666[   5<   +   5>   #   5@      5A      5C      5D      5E      5F      