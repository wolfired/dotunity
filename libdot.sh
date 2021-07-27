function DotSolutionNew() {
    local path_sln=${1:?"<解决方案路径>"}
    local name_sln=${2:?"<解决方案名称>"}

    mkdir -p $(dirname $path_sln)

    echo "正在创建解决方案: $path_sln"
    dotnet new sln -o $path_sln -n $name_sln
    echo "成功创建解决方案: $path_sln"
}

function DotSolutionDelete() {
    local path_sln=${1:?"<解决方案路径>"}
    local path_sln_built=${2:-} # [解决方案输出路径]

    if [[ -d $path_sln ]]; then
        echo "正在删除解决方案: $path_sln"
        rm -rf $path_sln/*.sln
        echo "成功删除解决方案: $path_sln"
    else
        echo "解决方案路径不存在, 无须删除: $path_sln"
    fi

    if [[ -d $path_sln_built ]]; then
        echo "正在删除解决方案输出路径: $path_sln_built"
        rm -rf $path_sln_built
        echo "成功删除解决方案输出路径: $path_sln_built"
    else
        echo "解决方案输出路径不存在, 无须删除: $path_sln_built"
    fi
}

function DotSolutionClean() {
    local path_sln=${1:?"<解决方案路径>"}
    local path_sln_built=${2:-} # [解决方案输出路径]

    if [[ -d $path_sln && -d $path_sln_built ]]; then
        echo "正在清理解决方案: $path_sln"
        dotnet clean -o $path_sln_built $path_sln
        echo "成功清理解决方案: $path_sln"
    elif [[ -d $path_sln ]]; then
        echo "正在清理解决方案: $path_sln"
        dotnet clean $path_sln
        echo "成功清理解决方案: $path_sln"
    else
        echo "解决方案路径不存在, 无须清理: $path_sln"
    fi
}

function DotProjectNew() {
    local path_sln=${1:?"<解决方案路径>"}
    local type_prj=${2:?"<Dotnet项目类型, classlib|console>"}
    local framework_prj=${3:?"<Dotnet项目框架, netstandard2.0|net5.0>"}
    local path_prj=${4:?"<Dotnet项目路径>"}

    local name_prj=$(basename $path_prj)

    if [[ -f $path_prj/$name_prj.csproj ]]; then
        echo "Dotnet项目已存在无须创建: $path_prj"
        return
    fi

    mkdir -p $(dirname $path_prj)

    dotnet new $type_prj --framework $framework_prj --force -o $path_prj -n $name_prj

    rm -rf $path_prj/*.cs

    mkdir -p $path_prj/src

    if [[ "classlib" == $type_prj && ! -f $path_prj/src/Lib.cs ]]; then
cat <<EOF > $path_prj/src/Lib.cs
using System;

namespace com.wolfired.$name_prj {
    public class Lib { }
}
EOF
    elif [[ "console" == $type_prj && ! -f $path_prj/src/Main.cs ]]; then
cat <<EOF > $path_prj/src/Main.cs
using System;

namespace com.wolfired.$name_prj
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
EOF
    fi

    dotnet sln $path_sln add $path_prj
}

function DotProjectAddPackages() {
    local path_prj=${1:?"<Dotnet项目路径>"}
    local name_packages=${2:?"依赖包名, 逗号分隔"}

    readarray -td, arr_name_package <<<"$name_packages,"
    unset 'arr_name_package[-1]'

    for name_package in "${arr_name_package[@]}"; do
        dotnet add $path_prj package $name_package
    done
}

function DotProjectAddReference() {
    local path_prj=${1:?"<Dotnet项目路径>"}
    local path_prj_depeds=${2:?"<Dotnet项目路径, 逗号分隔>"}

    readarray -td, arr_path_prj_deped <<<"$path_prj_depeds,"
    unset 'arr_path_prj_deped[-1]'

    for path_prj_deped in "${arr_path_prj_deped[@]}"; do
        dotnet add $path_prj reference $path_prj_deped
    done
}

function DotProjectDelete() {
    local path_prj=${1:?"<Dotnet项目路径>"}

    if [[ ! -d $path_prj ]]; then
        echo "Dotnet项目路径不存在无须删除: $path_prj"
        return
    fi

    echo "正在删除Dotnet项目: $path_prj"
    rm -rf $path_prj/bin $path_prj/obj $path_prj/*.csproj
    echo "成功删除Dotnet项目: $path_prj"
}

function DotProjectClean() {
    local path_prj=${1:?"<Dotnet项目路径>"}

    if [[ -d $path_prj ]]; then
        echo "正在清理Dotnet项目: $path_prj"
        dotnet clean $path_prj
        echo "成功清理Dotnet项目: $path_prj"
    else
        echo "Dotnet项目路径不存在, 无须清理: $path_prj"
    fi
}

function DotBuild() {
    local path_sln=${1:?"<解决方案路径|Dotnet项目路径>"}
    local path_out=${2:-} # [解决方案输出路径]

    if [[ -n $path_out && ! -d $path_out ]]; then
        mkdir -p $path_out
    fi

    if [[ -d $path_sln && -d $path_out ]]; then
        echo "正在构建: $path_sln"
        dotnet build -o $path_out $path_sln
        echo "成功构建: $path_sln"
    elif [[ -d $path_sln ]]; then
        echo "正在构建: $path_sln"
        dotnet build $path_sln
        echo "成功构建: $path_sln"
    else
        echo "路径不存在, 无须构建: $path_sln"
    fi
}

function DotDLLsSrc2Dst() {
    local path_src=${1:?"<DLLs来源路径>"}
    local path_dst=${2:?"<DLLs目标路径>"}
    local name_dlls=${3:?"<DLL文件名列表, 不包括后缀, 逗号分隔>"}
    local need_pdb=${4:?"<是否需要复制PDB文件, true|false>"}

    readarray -td, arr_name_dll <<<"$name_dlls,"
    unset 'arr_name_dll[-1]'

    for name_dll in "${arr_name_dll[@]}"; do
        cp -v -u $path_src/$name_dll.dll $path_dst/
        if [[ "true" == $need_pdb ]]; then
            cp -v -u $path_src/$name_dll.pdb $path_dst/
        fi
    done
}
