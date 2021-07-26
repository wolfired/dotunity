#!/bin/env bash

# root=$(dirname $(readlink -f $0)) # Used in Linux
root=E:/workspace_git/dotunity # Used in Window
root_name=$(basename $root)

source $root/libdot.sh
source $root/libunity.sh
source $root/libother.sh

path_sln=$root
name_sln=$root_name
path_out_dot=${path_out_dot:-$path_sln/out_dot}
path_out_u3d=${path_out_u3d:-$path_sln/out_u3d}
path_u3dprj=${path_u3dprj:-'E:/workspace_u3d/addrsrc'}



#---------------------------------------------------------------
dotnet build -o $path_out_dot $path_sln
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Editor/Plugins nopack_depend_editor,pack_depend_editor true
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Plugins nopack_depend_player,pack_depend_player true
