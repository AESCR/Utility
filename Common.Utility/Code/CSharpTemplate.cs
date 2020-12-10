using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Common.Utility.Code
{
    public class CSharpTemplate : ICodeTemplate
    {
        readonly StringBuilder codeBuilder;

        public CSharpTemplate()
        {
            codeBuilder = new StringBuilder();
        }

        public ICodeTemplate ImportReferences(params string[] references)
        {
            return ImportNamespace(references);
        }

        public ICodeTemplate ImportNamespace(params string[] names)
        {
            var result = names.ToList();
            result.Add("System");
            result.Add("System.Linq");
            result.Add("System.Text");
            var dResult = result.Distinct().ToList();
            foreach (var name in dResult)
            {
                codeBuilder.AppendLine($"using {name};");
            }

            return this;
        }

        public ICodeTemplate SetNamespace(string name)
        {
            codeBuilder.AppendLine();
            codeBuilder.AppendLine($"namespace {name}");
            codeBuilder.AppendLine("{");
            codeBuilder.AppendLine();
            codeBuilder.AppendLine("}");
            return this;
        }

        public ICodeTemplate SetProperty(string typeName,string propertyName)
        {
            string temp ="\t\t";
            temp+= $"public {typeName} {propertyName.First().ToString().ToUpper() + propertyName.Substring(1)}"+"{get;set;} ";
            var tabIndex= codeBuilder.ToString().LastIndexOf($" ", StringComparison.Ordinal);
            codeBuilder.Insert(tabIndex+1, temp);
            return this;
        }

        public ICodeTemplate SetInterfaceMethod(string methodName,string returnType,params string[] parameterTypes)
        {
            string temp = $"{Environment.NewLine}\t\tpublic {returnType} {methodName}";
            string inher = "(";
            for (var index = 0; index < parameterTypes.Length; index++)
            {
                string parameter = parameterTypes[index];
                if (index!=0)
                {
                    inher += ",";
                }
                inher =inher+ $"{parameter} {parameter.First().ToString().ToLower() + parameter.Substring(1)}";
            }
            inher += ");";
            temp = temp+inher+" ";
            var tabIndex= codeBuilder.ToString().LastIndexOf($" ", StringComparison.Ordinal);
            codeBuilder.Insert(tabIndex+1, temp);
            return this;
        }


        public ICodeTemplate SetClass(string className, bool isInterface=false,params string[] inheritors)
        {
            string temp = String.Empty;
            if (isInterface)
            {
                temp = $"\tpublic interface {className}";
            }
            else
            {
                temp = $"\tpublic class {className}";
            }
            string inher = "";
            for (var index = 0; index < inheritors.Length; index++)
            {
                string inheritor = inheritors[index];
                if (index!=0)
                {
                    inher += ",";
                }
                inher += inheritor;
            }
            if (inheritors.Length>0)
            {
                temp =temp+ ":"+inher;
            }
            temp += Environment.NewLine;
            temp+=$"\t{{ {Environment.NewLine}\t}}";
            var tabIndex= codeBuilder.ToString().LastIndexOf(" ", StringComparison.Ordinal);
            codeBuilder.Insert(tabIndex+1, temp);
            return this;
        }


        public void Save(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            var fName = Path.GetFileNameWithoutExtension(path);
            using (var fileStream = new FileStream($"{fName}.cs", FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                var bytes = Encoding.UTF8.GetBytes(codeBuilder.ToString());
                fileStream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}