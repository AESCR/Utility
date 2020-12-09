using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utility.Code
{
    /// <summary>
    /// OOL 代码模板生成器
    /// </summary>
    public interface ICodeTemplate
    {
        /// <summary>
        /// 导入库文件
        /// </summary>
        /// <param name="references"></param>
        ICodeTemplate ImportReferences(params string[] references);
        /// <summary>
        /// 导入命名空间
        /// </summary>
        ICodeTemplate ImportNamespace(params string[] name);
        /// <summary>
        /// 设置类命名空间
        /// </summary>
        /// <param name="name"></param>
        ICodeTemplate SetNamespace(string name);

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <returns></returns>
        ICodeTemplate SetProperty(string typeName,string propertyName);

        /// <summary>
        /// 添加无参数的方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="returnType"></param>
        /// <param name="parameterTypes"></param>
        /// <returns></returns>
        ICodeTemplate SetInterfaceMethod(string methodName, string returnType, params string[] parameterTypes);

        /// <summary>
        /// 添加类型 
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="inheritors">继承的类名</param>
        /// <param name="isInterface"></param>
        /// <returns></returns>
        ICodeTemplate SetClass(string className,bool isInterface=false,params string[] inheritors);


        void Save(string path);
    }
}