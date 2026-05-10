---
source_title: AWS RDS MySQL Parameters
source_url: https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html
engine: mysql
vendor: aws
topic: memory
seed_id: aws-rds-mysql-parameters
captured_at_utc: 2026-05-09T12:11:23.3185008+00:00
---

<!DOCTYPE html>
    <html xmlns="http://www.w3.org/1999/xhtml" lang="en-US"><head><meta http-equiv="Content-Type" content="text/html; charset=UTF-8" /><title>Parameters for MySQL - Amazon Relational Database Service</title><meta name="viewport" content="width=device-width,initial-scale=1" /><meta name="assets_root" content="/assets" /><meta name="target_state" content="Appendix.MySQL.Parameters" /><meta name="default_state" content="Appendix.MySQL.Parameters" /><link rel="icon" type="image/ico" href="/assets/images/favicon.ico" /><link rel="shortcut icon" type="image/ico" href="/assets/images/favicon.ico" /><link rel="canonical" href="https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" /><meta name="description" content="Learn about the parameters available for Amazon RDS for MySQL DB instances." /><meta name="deployment_region" content="IAD" /><meta name="product" content="Amazon Relational Database Service" /><meta name="guide" content="User Guide" /><meta name="abstract" content="Amazon Web Services (AWS) documentation to help you set up, operate, and scale a relational database in the AWS Cloud using Amazon Relational Database Service (Amazon RDS). You can create DB instances that run Amazon Aurora, MariaDB, Microsoft SQL Server, MySQL, Oracle, and PostgreSQL." /><meta name="guide-locale" content="en_us" /><meta name="tocs" content="toc-contents.json" /><link rel="canonical" href="https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" /><link rel="alternate" href="https://docs.aws.amazon.com/id_id/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="id-id" /><link rel="alternate" href="https://docs.aws.amazon.com/id_id/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="id" /><link rel="alternate" href="https://docs.aws.amazon.com/de_de/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="de-de" /><link rel="alternate" href="https://docs.aws.amazon.com/de_de/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="de" /><link rel="alternate" href="https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="en-us" /><link rel="alternate" href="https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="en" /><link rel="alternate" href="https://docs.aws.amazon.com/es_es/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="es-es" /><link rel="alternate" href="https://docs.aws.amazon.com/es_es/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="es" /><link rel="alternate" href="https://docs.aws.amazon.com/fr_fr/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="fr-fr" /><link rel="alternate" href="https://docs.aws.amazon.com/fr_fr/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="fr" /><link rel="alternate" href="https://docs.aws.amazon.com/it_it/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="it-it" /><link rel="alternate" href="https://docs.aws.amazon.com/it_it/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="it" /><link rel="alternate" href="https://docs.aws.amazon.com/ja_jp/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="ja-jp" /><link rel="alternate" href="https://docs.aws.amazon.com/ja_jp/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="ja" /><link rel="alternate" href="https://docs.aws.amazon.com/ko_kr/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="ko-kr" /><link rel="alternate" href="https://docs.aws.amazon.com/ko_kr/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="ko" /><link rel="alternate" href="https://docs.aws.amazon.com/pt_br/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="pt-br" /><link rel="alternate" href="https://docs.aws.amazon.com/pt_br/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="pt" /><link rel="alternate" href="https://docs.aws.amazon.com/zh_cn/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="zh-cn" /><link rel="alternate" href="https://docs.aws.amazon.com/zh_tw/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="zh-tw" /><link rel="alternate" href="https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" hreflang="x-default" /><meta name="feedback-item" content="RDS" /><meta name="this_doc_product" content="Amazon Relational Database Service" /><meta name="this_doc_guide" content="User Guide" /><head xmlns="http://www.w3.org/1999/xhtml"> <script defer="" src="/assets/r/awsdocs-doc-page.2.0.0.js"></script><link href="/assets/r/awsdocs-doc-page.2.0.0.css" rel="stylesheet"/></head>
<script defer="" id="awsc-panorama-bundle" type="text/javascript" src="https://prod.pa.cdn.uis.awsstatic.com/panorama-nav-init.js" data-config="{'appEntity':'aws-documentation','region':'us-east-1','service':'rds'}"></script><meta id="panorama-serviceSubSection" value="User Guide" /><meta id="panorama-serviceConsolePage" value="Parameters for MySQL" /></head><body class="awsdocs awsui"><div class="awsdocs-container"><p><a href="Appendix.MySQL.Parameters.md">View a markdown version of this page</a></p><awsdocs-header></awsdocs-header><awsui-app-layout id="app-layout" class="awsui-util-no-gutters" ng-controller="ContentController as $ctrl" header-selector="awsdocs-header" navigation-hide="false" navigation-width="$ctrl.navWidth" navigation-open="$ctrl.navOpen" navigation-change="$ctrl.onNavChange($event)" tools-hide="$ctrl.hideTools" tools-width="$ctrl.toolsWidth" tools-open="$ctrl.toolsOpen" tools-change="$ctrl.onToolsChange($event)"><div id="guide-toc" dom-region="navigation"><awsdocs-toc></awsdocs-toc></div><div id="main-column" dom-region="content" tabindex="-1"><awsdocs-view class="awsdocs-view"><div id="awsdocs-content"><head><title>Parameters for MySQL - Amazon Relational Database Service</title><meta name="pdf" content="/pdfs/AmazonRDS/latest/UserGuide/rds-ug.pdf#Appendix.MySQL.Parameters" /><meta name="rss" content="rdsupdates.rss" /><meta name="forums" content="https://repost.aws/tags/TAsibBK6ZeQYihN9as4S_psg" /><meta name="feedback" content="https://docs.aws.amazon.com/forms/aws-doc-feedback?hidden_service_name=RDS&amp;topic_url=https://docs.aws.amazon.com/en_us/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" /><meta name="feedback-yes" content="feedbackyes.html?topic_url=https://docs.aws.amazon.com/en_us/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" /><meta name="feedback-no" content="feedbackno.html?topic_url=https://docs.aws.amazon.com/en_us/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html" /><meta name="keywords" content="Amazon Relational Database Service,RDS,DB Instance" /><link rel="alternate" type="text/markdown" href="Appendix.MySQL.Parameters.md" title="Markdown version" /><script type="application/ld+json">
{
    "@context" : "https://schema.org",
    "@type" : "BreadcrumbList",
    "itemListElement" : [
      {
        "@type" : "ListItem",
        "position" : 1,
        "name" : "AWS",
        "item" : "https://aws.amazon.com"
      },
      {
        "@type" : "ListItem",
        "position" : 2,
        "name" : "Amazon RDS",
        "item" : "https://docs.aws.amazon.com/rds/index.html"
      },
      {
        "@type" : "ListItem",
        "position" : 3,
        "name" : "User Guide",
        "item" : "https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide"
      },
      {
        "@type" : "ListItem",
        "position" : 4,
        "name" : "Amazon RDS for MySQL",
        "item" : "https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/CHAP_MySQL.html"
      },
      {
        "@type" : "ListItem",
        "position" : 5,
        "name" : "Parameters for MySQL",
        "item" : "https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/CHAP_MySQL.html"
      }
    ]
}
</script></head><body><div id="main"><div style="display: none"><a href="/pdfs/AmazonRDS/latest/UserGuide/rds-ug.pdf#Appendix.MySQL.Parameters" target="_blank" rel="noopener noreferrer" title="Open PDF"></a></div><div id="breadcrumbs" class="breadcrumb"><a href="/index.html">Documentation</a><a href="/rds/index.html">Amazon RDS</a><a href="Welcome.html">User Guide</a></div><div id="main-content" class="awsui-util-container"><div id="main-col-body"><awsdocs-language-banner data-service="$ctrl.pageService"></awsdocs-language-banner><h1 class="topictitle" id="Appendix.MySQL.Parameters">Parameters for MySQL</h1><div class="awsdocs-page-header-container"><awsdocs-page-header></awsdocs-page-header><awsdocs-filter-selector id="awsdocs-filter-selector"></awsdocs-filter-selector></div><p>By default, a MySQL DB instance uses a DB parameter group that is specific to a MySQL database. 
        This parameter group contains parameters for the MySQL database engine. For information about 
        working with parameter groups and setting parameters, see <a href="./USER_WorkingWithParamGroups.html">Parameter groups for Amazon RDS</a>.</p><p>RDS for MySQL parameters are set to the default values of the storage engine that you have
        selected. For more information about MySQL parameters, see the <a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html" rel="noopener noreferrer" target="_blank"><span>MySQL
            documentation</span><awsui-icon class="awsdocs-link-icon" name="external"></awsui-icon></a>. For more information about MySQL storage engines, see <a href="./MySQL.Concepts.FeatureSupport.html#MySQL.Concepts.Storage">Supported storage engines for RDS for MySQL</a>.</p><p>You can view the parameters available for a specific RDS for MySQL version using the RDS console 
        or the AWS CLI. For information about viewing the parameters in a MySQL parameter group in the 
        RDS console, see <a href="./USER_WorkingWithParamGroups.Viewing.html">Viewing parameter values for a DB parameter group in Amazon RDS</a>.</p><p>Using the AWS CLI, you can view the parameters for an RDS for MySQL version by running the 
        <a href="https://docs.aws.amazon.com/cli/latest/reference/rds/describe-engine-default-parameters.html"><code class="code">describe-engine-default-parameters</code></a> command. Specify one of the following 
        values for the <code class="code">--db-parameter-group-family</code> option:</p><div class="itemizedlist">
         
         
         
    <ul class="itemizedlist"><li class="listitem">
            <p><code class="code">mysql8.4</code></p>
        </li><li class="listitem">
            <p><code class="code">mysql8.0</code></p>
        </li><li class="listitem">
            <p><code class="code">mysql5.7</code></p>
        </li></ul></div><p>For example, to view the parameters for RDS for MySQL version 8.0, run the following
        command.</p><pre class="programlisting"><div class="code-btn-container"><div class="btn-copy-code" title="Copy"><awsui-icon name="copy"></awsui-icon></div></div><!--DEBUG: cli (none)--><code class="nohighlight">aws rds describe-engine-default-parameters --db-parameter-group-family mysql8.0</code></pre><p>Your output looks similar to the following.</p><pre class="programlisting"><div class="code-btn-container"></div><!--DEBUG: cli ()--><code class=""><span>{</span>
    "EngineDefaults": <span>{</span>
        "Parameters": [
            <span>{</span>
                "ParameterName": "activate_all_roles_on_login",
                "ParameterValue": "0",
                "Description": "Automatically set all granted roles as active after the user has authenticated successfully.",
                "Source": "engine-default",
                "ApplyType": "dynamic",
                "DataType": "boolean",
                "AllowedValues": "0,1",
                "IsModifiable": true
            },
            <span>{</span>
                "ParameterName": "allow-suspicious-udfs",
                "Description": "Controls whether user-defined functions that have only an xxx symbol for the main function can be loaded",
                "Source": "engine-default",
                "ApplyType": "static",
                "DataType": "boolean",
                "AllowedValues": "0,1",
                "IsModifiable": false
            },
            <span>{</span>
                "ParameterName": "auto_generate_certs",
                "Description": "Controls whether the server autogenerates SSL key and certificate files in the data directory, if they do not already exist.",
                "Source": "engine-default",
                "ApplyType": "static",
                "DataType": "boolean",
                "AllowedValues": "0,1",
                "IsModifiable": false
            },            
        ...</code></pre><p>To list only the modifiable parameters for RDS for MySQL version 8.0, run the following
        command.</p><p>For Linux, macOS, or Unix:</p><pre class="programlisting"><div class="code-btn-container"><div class="btn-copy-code" title="Copy"><awsui-icon name="copy"></awsui-icon></div></div><!--DEBUG: cli (none)--><code class="nohighlight">aws rds describe-engine-default-parameters --db-parameter-group-family mysql8.0 \
   --query 'EngineDefaults.Parameters[?IsModifiable==`true`]'</code></pre><p>For Windows:</p><pre class="programlisting"><div class="code-btn-container"><div class="btn-copy-code" title="Copy"><awsui-icon name="copy"></awsui-icon></div></div><!--DEBUG: cli (none)--><code class="nohighlight">aws rds describe-engine-default-parameters --db-parameter-group-family mysql8.0 ^
   --query "EngineDefaults.Parameters[?IsModifiable==`true`]"</code></pre><awsdocs-copyright class="copyright-print"></awsdocs-copyright><awsdocs-thumb-feedback right-edge="{{$ctrl.thumbFeedbackRightEdge}}"></awsdocs-thumb-feedback></div><noscript><div><div><div><div id="js_error_message"><p><img src="https://d1ge0kk1l5kms0.cloudfront.net/images/G/01/webservices/console/warning.png" alt="Warning" /> <strong>Javascript is disabled or is unavailable in your browser.</strong></p><p>To use the Amazon Web Services Documentation, Javascript must be enabled. Please refer to your browser's Help pages for instructions.</p></div></div></div></div></noscript><div id="main-col-footer" class="awsui-util-font-size-0"><div id="doc-conventions"><a target="_top" href="/general/latest/gr/docconventions.html">Document Conventions</a></div><div class="prev-next"><div id="previous" class="prev-link" accesskey="p" href="./Appendix.MySQL.Options.memcached.html">memcached</div><div id="next" class="next-link" accesskey="n" href="./Appendix.MySQL.CommonDBATasks.html">Common DBA tasks for
            MySQL</div></div></div><awsdocs-page-utilities></awsdocs-page-utilities></div><div id="quick-feedback-yes" style="display: none;"><div class="title">Did this page help you? - Yes</div><div class="content"><p>Thanks for letting us know we're doing a good job!</p><p>If you've got a moment, please tell us what we did right so we can do more of it.</p><p><awsui-button id="fblink" rel="noopener noreferrer" target="_blank" text="Feedback" click="linkClick($event)" href="https://docs.aws.amazon.com/forms/aws-doc-feedback?hidden_service_name=RDS&amp;topic_url=https://docs.aws.amazon.com/en_us/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html"></awsui-button></p></div></div><div id="quick-feedback-no" style="display: none;"><div class="title">Did this page help you? - No</div><div class="content"><p>Thanks for letting us know this page needs work. We're sorry we let you down.</p><p>If you've got a moment, please tell us how we can make the documentation better.</p><p><awsui-button id="fblink" rel="noopener noreferrer" target="_blank" text="Feedback" click="linkClick($event)" href="https://docs.aws.amazon.com/forms/aws-doc-feedback?hidden_service_name=RDS&amp;topic_url=https://docs.aws.amazon.com/en_us/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html"></awsui-button></p></div></div></div></body></div></awsdocs-view><div class="page-loading-indicator" id="page-loading-indicator"><awsui-spinner size="large"></awsui-spinner></div></div><div id="tools-panel" dom-region="tools"><awsdocs-tools-panel id="awsdocs-tools-panel"></awsdocs-tools-panel></div></awsui-app-layout><awsdocs-cookie-banner class="doc-cookie-banner"></awsdocs-cookie-banner></div></body></html>
