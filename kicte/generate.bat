@echo off
setlocal
set kcp=%~dp0..\Kecakc.exe

echo Kicte - Kecaknoah Interop Class Template Engine
echo Auto Generator
echo -----------------------------------------------
echo;
mkdir generated > NUL 2>&1

for %%f in (*.txt) do (
  cd generated
  echo ========%%f
  %kcp% ../Kicte.kc ../%%f
  
  echo;
  cd ../
)

echo All templates has been successfully converted!
echo check the "generated" directory!
pause
endlocal