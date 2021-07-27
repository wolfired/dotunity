#!/bin/env bash

source $(dirname $0)/addrdst_inc.sh



#---------------------------------------------------------------
dotnet build -o $path_out_dot $path_sln
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Editor/Plugins nopack_depend_editor,pack_depend_editor true
DotDLLsSrc2Dst $path_out_dot $path_u3dprj/Assets/Plugins nopack_depend_player,pack_depend_player true
