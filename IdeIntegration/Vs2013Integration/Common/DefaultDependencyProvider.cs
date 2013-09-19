﻿using System.Linq;
using System;
using BoDi;
using EnvDTE;
using EnvDTE80;
using TechTalk.SpecFlow.BindingSkeletons;
using TechTalk.SpecFlow.IdeIntegration.Install;
using TechTalk.SpecFlow.IdeIntegration.Options;
using TechTalk.SpecFlow.IdeIntegration.Tracing;
using TechTalk.SpecFlow.Vs2010Integration.Install;
using TechTalk.SpecFlow.Vs2010Integration.LanguageService;
using TechTalk.SpecFlow.Vs2010Integration.Options;
using TechTalk.SpecFlow.Vs2010Integration.TestRunner;
using TechTalk.SpecFlow.Vs2010Integration.Tracing;
using TechTalk.SpecFlow.Vs2010Integration.Tracing.OutputWindow;
using TechTalk.SpecFlow.Vs2010Integration.Utils;

namespace TechTalk.SpecFlow.Vs2010Integration
{
    internal partial class DefaultDependencyProvider
    {
        static partial void RegisterCommands(IObjectContainer container);

        public virtual void RegisterDefaults(IObjectContainer container)
        {
            var serviceProvider = container.Resolve<IServiceProvider>();
            RegisterVsDependencies(container, serviceProvider);

            container.RegisterTypeAs<InstallServices, InstallServices>();
            container.RegisterTypeAs<VsBrowserGuidanceNotificationService, IGuidanceNotificationService>();
            container.RegisterTypeAs<WindowsFileAssociationDetector, IFileAssociationDetector>();
            container.RegisterTypeAs<RegistryStatusAccessor, IStatusAccessor>();

            container.RegisterTypeAs<IntegrationOptionsProvider, IIntegrationOptionsProvider>();
            container.RegisterInstanceAs<IIdeTracer>(VsxHelper.ResolveMefDependency<IVisualStudioTracer>(serviceProvider));
            container.RegisterInstanceAs(VsxHelper.ResolveMefDependency<IProjectScopeFactory>(serviceProvider));

            container.RegisterTypeAs<TestRunnerEngine, ITestRunnerEngine>();
            container.RegisterTypeAs<TestRunnerGatewayProvider, ITestRunnerGatewayProvider>();
            container.RegisterTypeAs<MsTestRunnerGateway, ITestRunnerGateway>(TestRunnerTool.VisualStudio2010MsTest.ToString());
            container.RegisterTypeAs<ReSharper5TestRunnerGateway, ITestRunnerGateway>(TestRunnerTool.ReSharper5.ToString());
            container.RegisterTypeAs<ReSharper6TestRunnerGateway, ITestRunnerGateway>(TestRunnerTool.ReSharper.ToString());
            container.RegisterTypeAs<SpecRunTestRunnerGateway, ITestRunnerGateway>(TestRunnerTool.SpecRun.ToString());
            container.RegisterTypeAs<VS2012RunnerGateway, ITestRunnerGateway>(TestRunnerTool.VisualStudio2012.ToString());
            container.RegisterTypeAs<AutoTestRunnerGateway, ITestRunnerGateway>(TestRunnerTool.Auto.ToString());

            container.RegisterTypeAs<StepDefinitionSkeletonProvider, IStepDefinitionSkeletonProvider>();
            container.RegisterTypeAs<DefaultSkeletonTemplateProvider, ISkeletonTemplateProvider>();
            container.RegisterTypeAs<StepTextAnalyzer, IStepTextAnalyzer>();

            RegisterCommands(container);
        }

        protected virtual void RegisterVsDependencies(IObjectContainer container, IServiceProvider serviceProvider)
        {
            var dte = serviceProvider.GetService(typeof(DTE)) as DTE;
            if (dte != null)
            {
                container.RegisterInstanceAs(dte);
                container.RegisterInstanceAs((DTE2)dte);
            }

            container.RegisterInstanceAs(VsxHelper.ResolveMefDependency<IOutputWindowService>(serviceProvider));
            container.RegisterInstanceAs(VsxHelper.ResolveMefDependency<IGherkinLanguageServiceFactory>(serviceProvider));
        }
    }
}