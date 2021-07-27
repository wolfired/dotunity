#!/bin/env bash

# root=$(dirname $(readlink -f $0)) # Used in Linux
root=E:/workspace_git/dotunity # Used in Window, hardcode
root_name=$(basename $root)

source $root/libdot.sh
source $root/libunity.sh
source $root/libother.sh

path_sln=$root
name_sln=$root_name
path_out_dot=${path_out_dot:-$path_sln/out_dot}
path_out_u3d=${path_out_u3d:-$path_sln/out_u3d}
path_u3dprj=${path_u3dprj:-'E:/workspace_u3d/addrdst'}



#---------------------------------------------------------------
#                                                    创建U3D项目
# UnityCreateProject $path_u3dprj/log_$(date +%y%m%d_%H%M%S).txt $path_u3dprj



#---------------------------------------------------------------
#                                                 Dotnet解决方案
DotSolutionNew $path_sln $name_sln



#---------------------------------------------------------------
#         Dotnet项目, 负责把U3D项目工程文件转换成Dotnet项目工程文件
name_dotprj=u3dcsprojcloner
DotProjectNew $path_sln console "net5.0" $path_sln/$name_dotprj
DotProjectAddPackages $path_sln/$name_dotprj Mono.Options
DotBuild $path_sln/$name_dotprj $path_out_dot



#---------------------------------------------------------------
#                                   Dotnet项目, U3D插件, 无包依赖
name_dotprj=nopack_depend_player
DotProjectNew $path_sln classlib "netstandard2.0" $path_sln/$name_dotprj

$path_out_dot/u3dcsprojcloner.exe \
--cfsrc $path_u3dprj/Assembly-CSharp.csproj \
--nssrc 'http://schemas.microsoft.com/developer/msbuild/2003' \
--cfdst $path_sln/$name_dotprj/$name_dotprj.csproj \
--skips nopack_depend_player \
--skips nopack_depend_editor \
--skips pack_depend_player \
--skips pack_depend_editor



#---------------------------------------------------------------
#                             Dotnet项目, U3D编辑器插件, 无包依赖
name_dotprj=nopack_depend_editor
DotProjectNew $path_sln classlib "netstandard2.0" $path_sln/$name_dotprj
DotProjectAddPackages $path_sln/$name_dotprj Mono.Options
DotProjectAddReference $path_sln/$name_dotprj $path_sln/nopack_depend_player

$path_out_dot/u3dcsprojcloner.exe \
--cfsrc $path_u3dprj/Assembly-CSharp-Editor.csproj \
--nssrc 'http://schemas.microsoft.com/developer/msbuild/2003' \
--cfdst $path_sln/$name_dotprj/$name_dotprj.csproj \
--skips nopack_depend_player \
--skips nopack_depend_editor \
--skips pack_depend_player \
--skips pack_depend_editor \
--skips 'Assembly-CSharp.csproj'

#                                构建Dotnet项目, 复制DLL到U3D项目
DotBuild $path_sln/$name_dotprj $path_out_dot
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Editor/Plugins nopack_depend_editor true
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Plugins nopack_depend_player true



#---------------------------------------------------------------
#                                                  安装U3D依赖包
# UnityExecuteMethod $path_u3dprj/log_$(date +%y%m%d_%H%M%S).txt $path_u3dprj com.wolfired.pack.Packer.Add --packs com.unity.addressables --packs com.unity.2d.sprite
# UnityExecuteMethod $path_u3dprj/log_$(date +%y%m%d_%H%M%S).txt $path_u3dprj com.wolfired.CommandLineExecuteMethods.GenU3DProjectFiles



#---------------------------------------------------------------
#                                            Dotnet项目, U3D插件
name_dotprj=pack_depend_player
DotProjectNew $path_sln classlib "netstandard2.0" $path_sln/$name_dotprj
DotProjectAddReference $path_sln/$name_dotprj $path_sln/nopack_depend_player

$path_out_dot/u3dcsprojcloner.exe \
--cfsrc $path_u3dprj/Assembly-CSharp.csproj \
--nssrc 'http://schemas.microsoft.com/developer/msbuild/2003' \
--cfdst $path_sln/$name_dotprj/$name_dotprj.csproj \
--skips nopack_depend_player \
--skips nopack_depend_editor \
--skips pack_depend_player \
--skips pack_depend_editor



#---------------------------------------------------------------
#                                       Dotnet项目, U3D编辑器插件
name_dotprj=pack_depend_editor
DotProjectNew $path_sln classlib "netstandard2.0" $path_sln/$name_dotprj
DotProjectAddPackages $path_sln/$name_dotprj Mono.Options
DotProjectAddReference $path_sln/$name_dotprj $path_sln/nopack_depend_player
DotProjectAddReference $path_sln/$name_dotprj $path_sln/nopack_depend_editor
DotProjectAddReference $path_sln/$name_dotprj $path_sln/pack_depend_player

$path_out_dot/u3dcsprojcloner.exe \
--cfsrc $path_u3dprj/Assembly-CSharp-Editor.csproj \
--nssrc 'http://schemas.microsoft.com/developer/msbuild/2003' \
--cfdst $path_sln/$name_dotprj/$name_dotprj.csproj \
--skips nopack_depend_player \
--skips nopack_depend_editor \
--skips pack_depend_player \
--skips pack_depend_editor \
--skips 'Assembly-CSharp.csproj'



#---------------------------------------------------------------
#                                构建Dotnet项目, 复制DLL到U3D项目
DotBuild $path_sln/$name_dotprj $path_out_dot
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Editor/Plugins pack_depend_editor true
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Plugins pack_depend_player true



#---------------------------------------------------------------
#
# UnityExecuteMethod $path_u3dprj/log_$(date +%y%m%d_%H%M%S).txt $path_u3dprj com.wolfired.CommandLineExecuteMethods.GenU3DProjectFiles
