   M S B A M L   `   `   `  ���� \  XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35`Dclr-namespace:System.Windows.Controls;assembly=PresentationFrameworkSystem.Windows.Controls   Doge   Doge.Portal   QControlsDclr-namespace:System.Windows.Controls;assembly=PresentationFramework  R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml        � Title$
  Doge��$	��600��$	��950��.���  � ColumnDefinitions �� $��1* �.+��� -   $	��tab��$��Stretch=�$��Stretch=�.@��� -   $��ti_login��$��登陆��.��� ���	#FFE5E5E5$��Stretch=�$��Stretch=� � RowDefinitions �� $��30 ��� $��30 ��� $��30 ��� $��30 ��� $��1* � �� $	��100 ��� $	��150 ��� $��1* �.+��� $��0��$��0��$��账号�� �� TextAlignment$ Right=�$��Center=��� $��1��$��0��$��密码��$ Right=�$��Center=��� $��2��$��0��$��	验证码��$ Right=�$��Center=��� -   $��txt_username��$��0��$��1��$	��145��$��25��G� -   $��txt_pwd��$��1��$��1��$	��145��$��25���� -   $��txt_botcode��$��2��$��1��$	��145��$��25���� -   $��txt_login_ready_msg��$��2��$��2��$��验证码获取中...��$
��Left=�$��Center=�$y�Hidden=��� -   $��img_botcode��$��2��$��2��$��60��$��20��$
��Left=�$y�Hidden=��� -   $��	btn_login��$��3��$��1��$��登陆��$��50��$��20��$
��Left=��� -	   $��ti_cust_info��$��客户账户信息��.��� ���	#FFE5E5E5$��Stretch=�$��Stretch=� �� $��1* ��� $��30 � �� $	��100 ��� $	��100 ��� $��1* �.+��� -
   $��txt_tci_accounts��$��2�� �� TextWrapping$
 Wrap=� �� AcceptsReturn . " �� VerticalScrollBarVisibility$ Visible=�$	��120��$
��Left=�$��1q��� $��1��$��Center=�$��Center=�.+��� -   $��btn_tci_search��$��查询��$��75��$��10q��� -   $��lv_tci_main��$��2��$��Stretch=�$��Stretch=� �� SelectionMode$ Multiple=� �� View �� .P��� $��	客户号��	 �� Width$		 100��
 �� DisplayMemberBinding
 ��  �� Path$ CustId��� $��	总资产��$		 100��
 �� $ 
TotalAsset��� $��服务人员��$		 100��
 �� $ Waiter��� $��客户经理��$		 100��
 �� $ Manager��� $��错误信息��$		 150��
 �� $ Error��� $��调试信息��$		 300��
 �� $ 	DebugInfo�� -   $��pb_tci_main��$��2��$��1���� -   $��ti_cust_bank��$��银行信息查询��.��� ���	#FFE5E5E5$��Stretch=�$��Stretch=� �� $��1* ��� $��30 � �� $	��100 ��� $	��100 ��� $��1* �.+��� -   $��txt_tcb_accounts��$��2��$
 Wrap=� . $ Visible=�$	��120��$
��Left=�$��1q��� $��1��$��Center=�$��Center=�.+��� -   $��btn_tcb_search��$��查询��$��75��$��10q��� -   $��lv_tcb_main��$��2��$��Stretch=�$��Stretch=�$ Multiple=� �� .P��� $��	客户号��$		 100��
 �� $ CustId��� $��开户日期��$		 100��
 �� $ OpenDate��� $��错误信息��$		 150��
 �� $ Error��� $��调试信息��$		 300��
 �� $ 	DebugInfo�� -   $��pb_tcb_main��$��2��$��1���� -   $��ti_cust_trade��$��交易指标查询��.��� ���	#FFE5E5E5$��Stretch=�$��Stretch=� �� $��1* ��� $��30 � �� $	��100 ��� $	��100 ��� $��1* �.+��� -   $��txt_tct_accounts��$��2��$
 Wrap=� . $ Visible=�$	��120��$
��Left=�$��1q��� $��1��$��Center=�$��Center=�.+��� -   $��btn_tct_search��$��查询��$��75��$��10q��� -   $��lv_tct_main��$��2��$��Stretch=�$��Stretch=�$ Multiple=� �� .P��� $��	客户号��$		 100��
 �� $ CustId��� $��	净佣金��$		 100��
 �� $ PureCommission��� $��错误信息��$		 150��
 �� $ Error��� $��调试信息��$		 300��
 �� $ 	DebugInfo�� -   $��pb_tct_main��$��2��$��1��