   M S B A M L   `   `   `  ����G  CClassTracker, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:ClassTracker.ViewsClassTracker.Views  5   	   A%clr-namespace:ClassTracker.ViewModelsClassTracker.ViewModels  5   	   (    "ClassTracker.Views.ApplicationView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:ClassTracker.Views     5   	   8local2%clr-namespace:ClassTracker.ViewModels     5   	     � Title$  Class Tracker��5	   	   $	��800��6   $
��1200��6,    � 	Resources 5
       �� DataType1  +ClassTracker.ViewModels.MainWindowViewModel$   ClassTracker.Views.DueListView2  ,ClassTracker.ViewModels.DescriptionViewModel(   "ClassTracker.Views.DescriptionView/  )ClassTracker.ViewModels.ScheduleViewModel%   ClassTracker.Views.ScheduleView��%�   (��       5      *, +)(�� 2     5      *, +)(�� d     5      *, +)�� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      5      .��5      ]� ��. .+�5   
   �� -   5         ��Name$ 
navigation��5      $
��Left=�6)   $��
5, 5, 0, 0q�6?   .+�5      �� $��Due List��6   $��
2, 5, 0, 0q�6G   ��6(   �� *
DueCommand+6G   �� 5      $��Descriptions��6   $��
2, 5, 0 ,0q�6S   ��6,   �� *DescriptionCommand+6S   �� 5      $��Schedule��6   $��
2, 5, 0 ,0q�6L   ��6(   �� *ScheduleCommand+6L   5      �� 5   
   -   5         ��Name$ Pages��5      $��Right=�6(   ��6?   �� *SelectedViewModel+5      5      