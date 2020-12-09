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
        ICodeTemplate ImportReferences(List<string> references);
        /// <summary>
        /// 导入命名空间
        /// </summary>
        ICodeTemplate ImportNamespace(List<string> name);
        /// <summary>
        /// 设置类命名空间
        /// </summary>
        /// <param name="name"></param>
        ICodeTemplate SetNamespace(string name);
        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        ICodeTemplate AddProperty(params string[] property);
        /// <summary>
        /// 添加无参数的方法
        /// </summary>
        /// <param name="methodName"></param>
        /// <returns></returns>
        ICodeTemplate AddMthod<T>(string methodName);
        /// <summary>
        /// 添加类型 
        /// </summary>
        /// <param name="className">类名</param>
        /// <param name="inheritors">继承的类名</param>
        /// <returns></returns>
        ICodeTemplate AddClass(string className,params string[] inheritors);
        /// <summary>
        /// 查询定位输入流位置
        /// </summary>
        void Reposition();
    }
}