   M S B A M L   `   `   `  ����G  CClassesForNP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA0clr-namespace:ClassesForNP;assembly=ClassesForNPClassesForNP  5   	    WpfApp1"   WpfApp1.Views.AllUsersWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Blocal0clr-namespace:ClassesForNP;assembly=ClassesForNP     5   	     � Title$  AllUsersWindow��5   	   $	��400��6    $	��300��6-    � 	Resources 5	         listBoxTemplate �� DataType   ClassesForNP.WUser �� Path��%�  &	        �� 5
   
   "  60   .R�5      �� $��3q�6   .+�5      ]� .+�5      �� $
��Bold"�6    $��Name:��62   $
��Left=�6?   $��5,0,10,0q�6U   �� 5      $�� ��6    �� 5      
��� � �$
��Bold"�6R   ��6    �� $ NickName�6R   5      ]� 5      .+�5      �� $
��Bold"�6    $��State:��62   
��� ���$
��Left=�6X   $��5,0,5,0q�6n   �� 5      ��6    �� $ Email�5      5      5      5      .��5      ]� .+�5   
   �� -   5        $��addContactB��5      $	��Add��6$   $	��Top=�62   ��6G   �� *
AddContact+��6g   ��  �� ElementName$ allContactsLb��$ SelectedIndex��� 5   
   -   5        $��allContactsLb��5      #��[  6N   ��6(   �� $ Contacts�6N   5      5      