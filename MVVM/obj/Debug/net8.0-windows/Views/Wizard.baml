   M S B A M L   `   `   `  ����?  ;MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:MVVM.Views
MVVM.Views  5   	       MVVM.Views.Wizard   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:MVVM.Views     5   	   $	��640��5   	   $	��350��6     � WindowStartupLocation$  CenterOwner=�6"    � 
ResizeMode$ NoResize=�5	   	    � ShowInTaskbar$ False��6   .��5
      � .+�5   
   �� -   5         ��Name$ Step1��5      $��0��6$   $��Center=�61   y�6L   �� *IsStep1Visible+.+�5      ��  �� Orientation$ 
Horizontal=�6   $��Center=�62   $��0,0,0,10q�6O   .+�5      �� $��Title:��6   $��Center=�6*   $��80��6E    �� TextAlignment$ Right=�6P   �� 5      $	��200��6   $��30��6&   ��62   �� *ProjectTitle+5      �� 5      $ 
Horizontal=�6   $��Center=�62   $��0,0,0,10q�6O   .+�5      �� $��Start Date:��6   $��Center=�6/   $��80��6J   $ Right=�6U   (  "System.Windows.Controls.DatePicker  5      $	��200��6      SelectedDate 6)   �� *	DateStart+5      �� 5      $ 
Horizontal=�6   $��Center=�62   $��0,0,0,10q�6O   .+�5      �� $��	End Date:��6   $��Center=�6-   $��80��6H   $ Right=�6S     5      $	��200��6    6)   �� *	DateEnd+5      �� 5      $ 
Horizontal=�6   $��Center=�62   $��0,0,0,10q�6O   .+�5      �� $��	Priority:��6   $��Center=�6-   $��80��6H   $ Right=�6S   �� 5      $	��200��6   ��6&   �� *
Priority+5       5!      �� 5#   
   -   5        $ Step2��5#      $��0��6$   $ Vertical=�61   $��Center=�6H   $��Center=�6e   y�6�   �� *IsStep2Visible+.+�5$      �� $ 
Horizontal=�6   $��Center=�62   $��0,0,0,10q�6O   .+�5%      �� $��	Customer:��6   $��Center=�6-   $��80��6H   $ Right=�6S   �� 5&      $	��200��6    �� DisplayMemberPath$ CompanyName��6I    �� 
IsEditable$
 True��6�   ��6'   �� *	Customers+	 �� SelectedItem	 6i   �� *SelectedCustomer+
 �� Text
 6�   �� *TextForSearchCustomer+5'      �� 5)      $ 
Horizontal=�6   $��Center=�62   $��0,0,0,10q�6O   .+�5*      �� $��	Executor:��6   $��Center=�6-   $��80��6H   $ Right=�6S   �� 5+      $	��200��6   $ CompanyName��6I   $
 True��6�   ��6'   �� *	Executors+	 6i   �� *SelectedExecutor+
 6�   �� *TextForSearchExecutor+5,      5-      �� 5/   
   -   5        $ Step3��5/      $��0��6$   $ 
Horizontal=�61   $��Center=�6J   y�6g   �� *IsStep3Visible+.+�50      �� $��Manager:��6   $��Center=�6(   $��80��6C   $ Right=�6N   �� 51      $	��200��6   $��30��6#   $ FullName��6Q   $
 True��6�   ��6/   �� *	Employees+	 6n   �� *SelectedManager+
 6�   �� *TextForSearchManager+52      �� 54   
   -   5        $ Step4��54      $��0��6$   $ Vertical=�61   $��Center=�6H   y�6e   �� *IsStep4Visible+.+�55      �� $+��%Choose employees to join the project:��6   $	��600��6E   �� 56      $ 
Horizontal=�6   $��Center=�63   .+�57      �� $ Vertical=�6   .+�58      &   System.Windows.Controls.DataGrid  $	��300��6   $	��170��6+      SelectionMode$ Extended=�68   ��6Q   �� *	Employees+	 6s   �� *SelectedEmployee+5:      �� 5;      $(��"Add selected employee to team list��6   $	��300��6J   $��30��6V   $��Center=�6b   $��
10,10,10,0q�6   ��6�   �� *JoinEmployeeCommand+�� 5<      $+��%Remove selected employee to team list��6   $	��300��6M   $��30��6Y   $��Center=�6e   $��
10,10,10,0q�6�   ��6�   �� *RemoteEmployeeCommand+5=      �� 5>      ${�False��6   $	��300��6,   $ FullName��6b   ��68   �� *SelectedEmployees+6b   5?      5@      �� 5A   
   $ 
Horizontal=�6   $��Center=�6/   $��Bottom=�6L   $��0 0 0 0q�6g   .+�5B      �� $��Previous��6   $	��100��6(   ��64   �� *PreviousCommand+�� 5C      $
��Next��6   $	��100��6$   $��10,0,0,0q�6z   ��60   �� *NextCommand+{�6P   �� *IsNextButtonEnabled+6z   �� 5D      $��Finish��6   $	��100��6&   ��62   �� *FinishCommand+y�6T   �� *IsStep4Visible+5E      5F      5G      