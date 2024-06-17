set WORKSPACE=..
set LUBAN_DLL=%WORKSPACE%\Tools\Luban\Luban.dll
set CONF_ROOT=.
set OUTPUT_DATA_PATH=%WORKSPACE%\..\Assets\GameMain\LubanConfigs
set OUTPUT_CODE_PATH=%WORKSPACE%\..\Assets\GameMain\Scripts\LubanConfig

dotnet %LUBAN_DLL% ^
    -t all ^
    -c cs-simple-json ^
    -d json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputDataDir=%OUTPUT_DATA_PATH% ^
    -x outputCodeDir=%OUTPUT_CODE_PATH%

pause