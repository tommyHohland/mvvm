   M S B A M L   `   `   `  ����?  ;MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MVVM.Views.PagesMVVM.Views.Pages  5      #    MVVM.Views.Pages.EmployeePage   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      0localclr-namespace:MVVM.Views.Pages     5        N� Title$   ��5      $	��720��6   $
��1280��6   .��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid     AutoGenerateColumns$ False��5         CanUserAddRows$
 True��5         CanUserDeleteRows$ False��5         SelectionMode$ Single=�5      $��10q�5      ��5      �� *	Employees+ �� SelectedItem 5      �� *SelectedEmployee+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ 	Full Name��6%   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 68   �� *
FullName+  5      $ Email��6%   	 64   �� *Email+5      �� 5   
   $��1��6   
 �� Orientation$
 
Horizontal=�6"   $��Center=�6;   $��10q�6X   .+�5      �� $��Save All Changes��6   $��5q�6W   ��60   �� *SaveChangesCommand+6W   �� 5      $��Delete Selected��6   $��5q�6Y   ��6/   �� *DeleteEmployeeCommand+6Y   5      5      5       