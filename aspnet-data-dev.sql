SET IDENTITY_INSERT [dbo].[Masters] ON 

INSERT [dbo].[Masters] ([MasterID], [RegisterDate], [Resturant], [Email], [Phone], [Contact], [OrgNr], [AppId], [MasterKey]) VALUES (1, CAST(N'2017-02-23' AS Date), N'kdrGold   ', N'kdr@kdr.no', CAST(92627034 AS Numeric(18, 0)), N'øivind    ', N'hehehehe  ', N'cXGkFRPJK9s:APA91bE4MoI9osmgcV1pqp_Zw30uKAb1b_eQy_PHkodGa7ktsl2YGYXw4Px0RE-5lwa6ftMZZ7iAuQTHJUtrDZETUmnFjZys9BYnKxwIVe_Rac3BalHgrshg6WPdJ0IwXbQLezFYvwmf', N'NoMaster')
INSERT [dbo].[Masters] ([MasterID], [RegisterDate], [Resturant], [Email], [Phone], [Contact], [OrgNr], [AppId], [MasterKey]) VALUES (11, CAST(N'2017-04-04' AS Date), N'Døgnvill', N'døgn@vill.com', CAST(92627034 AS Numeric(18, 0)), N'øivind S Heggland', N'no org    ', N'cG1KWs0ynwU:APA91bHqzSQD-LCXnPjRDWYTKLWmN7GMpOqoaFPdoEf1tG4DwGMxfpS1oEqW5M6QoEcEahmPHB63wseDnwGlTjlD0x6Nk1AvS_0iSIsY5k-zTyWEXTnKuO5T1jh81ttTqjpuFsdNxd3M', N'DøgcG1K')
SET IDENTITY_INSERT [dbo].[Masters] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [UserName], [MasterKey], [Active], [AppId]) VALUES (0, N'jan       ', N'none', 0, N'chL8ZJDaVqo:APA91bEJSgPXK8K4NylUuEbydI7ClCGRR-rJOq7wF83y8aae2Nv_tS1rzXP7Ri_z5opBZV6AjwmDWkwOEB4W0-RSLILXM7BEdEU4dJyugAX4axM91mBsQniU7jCOADnA7eUtWtvLwmyj')
INSERT [dbo].[Users] ([UserId], [UserName], [MasterKey], [Active], [AppId]) VALUES (1, N'oivheg    ', N'DøgcG1K', 1, N'c4T1NHfgRpU:APA91bGab7_3r-4idhnXjbabXGCyZKKpRm87DwO52hxAMX9J_mV44W9_x_MUBWUTyBdV67PDi7Rjq5S_kOkrMA-7sUtA9T_R_RX9tyQdg7OYXDINQFVx9BCZssSD5CmEZgyrgMZZlysE')
INSERT [dbo].[Users] ([UserId], [UserName], [MasterKey], [Active], [AppId]) VALUES (3, N'espen     ', N'none', 0, N'e7tKV_yDbBM:APA91bF6d5O33RB7TUGwL5moop6_QxdjpzsUAxHWBqWdNeuuCyzwcAR3LYP__3tj7O-vLINM0Eo7dacW3fMUMkU5BiyNi3e05ns_thrVw2WJo8TvZ76NGSnig0uGGxcsTlO79aX-bZlZ')
INSERT [dbo].[Users] ([UserId], [UserName], [MasterKey], [Active], [AppId]) VALUES (42, N'ovland    ', N'DøgcG1K', 1, N'dnTClPUQLK8:APA91bEx8sxYaKhK1OQMgvBwzpuxsVVS8wjGcIAi_nL-uOtUG_4KY7qAnlEj1r9vxxAJMVDfDf9cCbDcPgJ6yw4hDJ9Vs7n008tc76Y1HobwXWbOBGyiayUKTwkYOUM0_G6xCillRAsl')
INSERT [dbo].[Users] ([UserId], [UserName], [MasterKey], [Active], [AppId]) VALUES (43, N'heggland  ', N'døgcg1k', 1, N'ct671s0z7Aw:APA91bHkBCBgGBVc_f8d0Hxi79smzvQ5aA84NErvpg2SznReIAmejzsQh6_D_b5xhzeXJ1J7vhVDJpaekbAq6JuRQ9FfYEbW0aqy9XAWq50aNWR-cxB6aBPz8ed-hDO9BhZQ-tjFZ-MR')
SET IDENTITY_INSERT [dbo].[Users] OFF
