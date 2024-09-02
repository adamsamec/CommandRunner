"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.8 Tools\ResGen.exe" CommandRunner\Resources\Resources.resx /str:cs,CommandRunner /publicclass
move /y CommandRunner\Resources\Resources.cs CommandRunner\Resources\Resources.Designer.cs
del CommandRunner\Resources\Resources.resources
title Resources generated
pause

