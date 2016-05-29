using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml;
using Xunit.Sdk;

namespace Xunit.MEF
{
    /// <summary>
    /// Wraps a test command
    /// </summary>
    public class TestCommandWrapper : ITestCommand, IMethodInfo
    {
        private ITestCommand _testCommand;
        private IMethodInfo _method;

        public TestCommandWrapper(ITestCommand testCommand,IMethodInfo method)
        {
            _testCommand = testCommand;
            _method = method;
        }

        public ITestCommand TestCommand { get { return _testCommand; } }

        public IMethodInfo Method { get { return _method; } }


        #region ITestCommand Members
        public string DisplayName
        {
            get
            {
                return _testCommand.DisplayName;
            }
        }

        public bool ShouldCreateInstance
        {
            get
            {
                return _testCommand.ShouldCreateInstance;
            }
        }

        public int Timeout
        {
            get
            {
                return _testCommand.Timeout;
            }
        }

        public MethodResult Execute(object testClass)
        {
            var parameters = MethodInfo.GetParameters();
            if (parameters == null || parameters.Length == 0)
            {
                return _testCommand.Execute(testClass);
            }
            else
            {
                var dependencyResolver = ContainerResolver.GetDependencyResolver(Class.Type);
                var arrparameters = new object[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    arrparameters[i] = dependencyResolver.GetType(parameters[i].ParameterType);
                }
                Invoke(testClass, arrparameters);
                return new PassedResult(_method, DisplayName);
            }
        }

        public XmlNode ToStartXml()
        {
            return _testCommand.ToStartXml();
        }
        #endregion


        #region IMethodInfo Members
        public ITypeInfo Class
        {
            get
            {
                return _method.Class;
            }
        }

        public bool IsAbstract
        {
            get
            {
                return _method.IsAbstract;
            }
        }

        public bool IsStatic
        {
            get
            {
                return _method.IsStatic;
            }
        }

        public MethodInfo MethodInfo
        {
            get
            {
                return _method.MethodInfo;
            }
        }

        public string Name
        {
            get
            {
                return _method.Name;
            }
        }

        public string ReturnType
        {
            get
            {
                return _method.ReturnType;
            }
        }

        public string TypeName
        {
            get
            {
                return _method.TypeName;
            }
        }

        public object CreateInstance()
        {
            return _method.CreateInstance();
        }

        public IEnumerable<IAttributeInfo> GetCustomAttributes(Type attributeType)
        {
            return _method.GetCustomAttributes(attributeType);
        }

        public bool HasAttribute(Type attributeType)
        {
            return _method.HasAttribute(attributeType);
        }

        public void Invoke(object testClass, params object[] parameters)
        {
            _method.Invoke(testClass, parameters);
        } 
        #endregion
    }
}
