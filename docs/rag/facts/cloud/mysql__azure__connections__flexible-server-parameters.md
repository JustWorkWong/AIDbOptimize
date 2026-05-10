---
source_title: Azure Database for MySQL Flexible Server Parameters
source_url: https://learn.microsoft.com/azure/mysql/flexible-server/concepts-server-parameters
engine: mysql
vendor: azure
topic: connections
seed_id: azure-mysql-flexible-server-parameters
captured_at_utc: 2026-05-09T12:11:24.5744616+00:00
---

 <!DOCTYPE html>
		<html
			class="layout layout-holy-grail   show-table-of-contents conceptual show-breadcrumb default-focus"
			lang="en-us"
			dir="ltr"
			data-authenticated="false"
			data-auth-status-determined="false"
			data-target="docs"
			x-ms-format-detection="none"
		>
			
		<head>
			<title>Server Parameters in Azure Database for MySQL - Flexible Server | Microsoft Learn</title>
			<meta charset="utf-8" />
			<meta name="viewport" content="width=device-width, initial-scale=1.0" />
			<meta name="color-scheme" content="light dark" />

			<meta name="description" content="This article provides guidelines for configuring server parameters in Azure Database for MySQL - Flexible Server." />
			<link rel="canonical" href="https://learn.microsoft.com/en-us/azure/mysql/flexible-server/concepts-server-parameters" /> 

			<!-- Non-customizable open graph and sharing-related metadata -->
			<meta name="twitter:card" content="summary_large_image" />
			<meta name="twitter:site" content="@MicrosoftLearn" />
			<meta property="og:type" content="website" />
			<meta property="og:image:alt" content="Microsoft Learn" />
			<meta property="og:image" content="https://learn.microsoft.com/en-us/media/open-graph-image.png" />
			<!-- Page specific open graph and sharing-related metadata -->
			<meta property="og:title" content="Server Parameters in Azure Database for MySQL - Flexible Server" />
			<meta property="og:url" content="https://learn.microsoft.com/en-us/azure/mysql/flexible-server/concepts-server-parameters" />
			<meta property="og:description" content="This article provides guidelines for configuring server parameters in Azure Database for MySQL - Flexible Server." />
			<meta name="platform_id" content="d0357407-f539-acdb-ea93-516cbfac8e93" /> <meta name="scope" content="Azure" />
			<meta name="locale" content="en-us" />
			 
			<meta name="uhfHeaderId" content="azure" />

			<meta name="page_type" content="conceptual" />

			<!--page specific meta tags-->
			

			<!-- custom meta tags -->
			
		<meta name="breadcrumb_path" content="../../breadcrumb/azure-databases/toc.json" />
	
		<meta name="feedback_help_link_url" content="/answers/tags/181/azure-database-mysql/" />
	
		<meta name="feedback_help_link_type" content="get-help-at-qna" />
	
		<meta name="feedback_product_url" content="https://feedback.azure.com/d365community/forum/47b1e71d-ee24-ec11-b6e6-000d3a4f0da0" />
	
		<meta name="feedback_system" content="Standard" />
	
		<meta name="permissioned-type" content="public" />
	
		<meta name="recommendations" content="true" />
	
		<meta name="recommendation_types" content="Training" />
	
		<meta name="recommendation_types" content="Certification" />
	
		<meta name="ms.suite" content="office" />
	
		<meta name="learn_banner_products" content="azure" />
	
		<meta name="author" content="VandhanaMehta" />
	
		<meta name="ms.author" content="vamehta" />
	
		<meta name="ms.reviewer" content="maghan" />
	
		<meta name="ms.date" content="2025-11-25T00:00:00Z" />
	
		<meta name="ms.service" content="azure-database-mysql" />
	
		<meta name="ms.subservice" content="flexible-server" />
	
		<meta name="ms.topic" content="concept-article" />
	
		<meta name="document_id" content="93311ddf-819c-d557-0fc6-8075b475b9f3" />
	
		<meta name="document_version_independent_id" content="51a3a69f-1092-5fcc-df8f-d525fcdc9b71" />
	
		<meta name="updated_at" content="2026-04-27T17:17:00Z" />
	
		<meta name="original_content_git_url" content="https://github.com/MicrosoftDocs/azure-databases-docs-pr/blob/live/articles/mysql/flexible-server/concepts-server-parameters.md" />
	
		<meta name="gitcommit" content="https://github.com/MicrosoftDocs/azure-databases-docs-pr/blob/697b1a945f08c67a629d32a8121df32b99099cf0/articles/mysql/flexible-server/concepts-server-parameters.md" />
	
		<meta name="git_commit_id" content="697b1a945f08c67a629d32a8121df32b99099cf0" />
	
		<meta name="site_name" content="Docs" />
	
		<meta name="depot_name" content="Learn.azure-databases" />
	
		<meta name="schema" content="Conceptual" />
	
		<meta name="toc_rel" content="../toc.json" />
	
		<meta name="word_count" content="3852" />
	
		<meta name="asset_id" content="mysql/flexible-server/concepts-server-parameters" />
	
		<meta name="moniker_range_name" content="" />
	
		<meta name="item_type" content="Content" />
	
		<meta name="source_path" content="articles/mysql/flexible-server/concepts-server-parameters.md" />
	
		<meta name="previous_tlsh_hash" content="88B25DA2A63CCA01EE930E1379AEEB74E4F1CC8CA6743DCC166817B2DA1F6C6E1B5D2CBBEB0BBB0C377249521196691E91C2EB2960FC7228552D496EDB09155356C93BBDC0" />
	
		<meta name="github_feedback_content_git_url" content="https://github.com/MicrosoftDocs/azure-databases-docs/blob/main/articles/mysql/flexible-server/concepts-server-parameters.md" />
	
		<meta name="markdown_url" content="https://learn.microsoft.com/en-us/azure/mysql/flexible-server/concepts-server-parameters?accept=text/markdown" />
	 
		<meta name="cmProducts" content="https://authoring-docs-microsoft.poolparty.biz/devrel/4c3c562b-e580-487f-8ab7-ed723105d8cf" data-source="generated" />
	
		<meta name="cmProducts" content="https://authoring-docs-microsoft.poolparty.biz/devrel/68ec7f3a-2bc6-459f-b959-19beb729907d" data-source="generated" />
	
		<meta name="spProducts" content="https://authoring-docs-microsoft.poolparty.biz/devrel/c5148132-a95f-467e-b31c-8786a8264397" data-source="generated" />
	
		<meta name="spProducts" content="https://authoring-docs-microsoft.poolparty.biz/devrel/90370425-aca4-4a39-9533-d52e5e002a5d" data-source="generated" />
	

			<!-- assets and js globals -->
			
			<link rel="stylesheet" href="/static/assets/0.4.03413.7809-3b89a05c/styles/site.css" />
			
			<script src="https://wcpstatic.microsoft.com/mscc/lib/v2/wcp-consent.js"></script>
			<script src="https://js.monitor.azure.com/scripts/c/ms.jsll-4.min.js"></script>
			<script src="/_themes/docs.theme/master/en-us/_themes/global/deprecation.js"></script>

			<!-- msdocs global object -->
			<script id="msdocs-script">
		var msDocs = {
  "environment": {
    "accessLevel": "online",
    "azurePortalHostname": "portal.azure.com",
    "reviewFeatures": false,
    "supportLevel": "production",
    "systemContent": true,
    "siteName": "learn",
    "legacyHosting": false
  },
  "data": {
    "contentLocale": "en-us",
    "contentDir": "ltr",
    "userLocale": "en-us",
    "userDir": "ltr",
    "pageTemplate": "Conceptual",
    "brand": "azure",
    "context": {},
    "standardFeedback": true,
    "showFeedbackReport": false,
    "feedbackHelpLinkType": "get-help-at-qna",
    "feedbackHelpLinkUrl": "/answers/tags/181/azure-database-mysql/",
    "feedbackSystem": "Standard",
    "feedbackGitHubRepo": "MicrosoftDocs/azure-docs",
    "feedbackProductUrl": "https://feedback.azure.com/d365community/forum/47b1e71d-ee24-ec11-b6e6-000d3a4f0da0",
    "extendBreadcrumb": false,
    "isEditDisplayable": true,
    "isPrivateUnauthorized": false,
    "hideViewSource": false,
    "isPermissioned": false,
    "hasRecommendations": true,
    "contributors": [
      {
        "name": "VandhanaMehta",
        "url": "https://github.com/VandhanaMehta"
      },
      {
        "name": "Albertyang0",
        "url": "https://github.com/Albertyang0"
      },
      {
        "name": "SudheeshGH",
        "url": "https://github.com/SudheeshGH"
      },
      {
        "name": "markingmyname",
        "url": "https://github.com/markingmyname"
      },
      {
        "name": "siddsawant",
        "url": "https://github.com/siddsawant"
      },
      {
        "name": "xboxeer",
        "url": "https://github.com/xboxeer"
      },
      {
        "name": "hyoshioka0128",
        "url": "https://github.com/hyoshioka0128"
      },
      {
        "name": "HJToland3",
        "url": "https://github.com/HJToland3"
      },
      {
        "name": "v-alje",
        "url": "https://github.com/v-alje"
      },
      {
        "name": "LifeRyuto",
        "url": "https://github.com/LifeRyuto"
      },
      {
        "name": "aditiiitb",
        "url": "https://github.com/aditiiitb"
      },
      {
        "name": "rothja",
        "url": "https://github.com/rothja"
      },
      {
        "name": "brown-hub",
        "url": "https://github.com/brown-hub"
      },
      {
        "name": "savjani",
        "url": "https://github.com/savjani"
      },
      {
        "name": "ambhatna",
        "url": "https://github.com/ambhatna"
      }
    ]
  },
  "functions": {}
};;
	</script>

			<!-- base scripts, msdocs global should be before this -->
			<script src="/static/assets/0.4.03413.7809-3b89a05c/scripts/en-us/index-docs.js"></script>
			

			<!-- json-ld -->
			
		</head>
	
			<body
				id="body"
				data-bi-name="body"
				class="layout-body "
				lang="en-us"
				dir="ltr"
			>
				<header class="layout-body-header background-color-body-medium">
		<div class="header-holder has-default-focus">
			
		<a
			href="#main"
			
			style="z-index: 1070"
			class="outline-color-text visually-hidden-until-focused position-fixed inner-focus focus-visible top-0 left-0 right-0 padding-xs text-align-center background-color-body"
			
		>
			Skip to main content
		</a>
	
		<a
			href="#"
			data-skip-to-ask-learn
			style="z-index: 1070"
			class="outline-color-text visually-hidden-until-focused position-fixed inner-focus focus-visible top-0 left-0 right-0 padding-xs text-align-center background-color-body"
			hidden
		>
			Skip to Ask Learn chat experience
		</a>
	

			<div hidden id="cookie-consent-holder" data-test-id="cookie-consent-container"></div>
			<!-- Unsupported browser warning -->
			<div
				id="unsupported-browser"
				style="background-color: white; color: black; padding: 16px; border-bottom: 1px solid grey;"
				hidden
			>
				<div style="max-width: 800px; margin: 0 auto;">
					<p style="font-size: 24px">This browser is no longer supported.</p>
					<p style="font-size: 16px; margin-top: 16px;">
						Upgrade to Microsoft Edge to take advantage of the latest features, security updates, and technical support.
					</p>
					<div style="margin-top: 12px;">
						<a
							href="https://go.microsoft.com/fwlink/p/?LinkID=2092881 "
							style="background-color: #0078d4; border: 1px solid #0078d4; color: white; padding: 6px 12px; border-radius: 2px; display: inline-block;"
						>
							Download Microsoft Edge
						</a>
						<a
							href="https://learn.microsoft.com/en-us/lifecycle/faq/internet-explorer-microsoft-edge"
							style="background-color: white; padding: 6px 12px; border: 1px solid #505050; color: #171717; border-radius: 2px; display: inline-block;"
						>
							More info about Internet Explorer and Microsoft Edge
						</a>
					</div>
				</div>
			</div>
			<!-- site header -->
			<div
				id="ms--site-header"
				data-test-id="site-header-wrapper"
				itemscope="itemscope"
				itemtype="http://schema.org/Organization"
			>
				<div
					id="ms--mobile-nav"
					class="site-header display-none-tablet padding-inline-none gap-none"
					data-bi-name="mobile-header"
					data-test-id="mobile-header"
				></div>
				<div
					id="ms--primary-nav"
					class="site-header display-none display-flex-tablet"
					data-bi-name="L1-header"
					data-test-id="primary-header"
				></div>
				<div
					id="ms--secondary-nav"
					class="site-header display-none display-flex-tablet"
					data-bi-name="L2-header"
					data-test-id="secondary-header"
					
				></div>
			</div>
			
		<!-- banner -->
		<div data-banner>
			<div id="disclaimer-holder"></div>
			
		</div>
		<!-- banner end -->
	
		</div>
	</header>
				 <section
					id="layout-body-menu"
					class="layout-body-menu border-right display-flex background-color-body-medium"
					data-bi-name="menu"
			  >
					
		<div
			id="left-container"
			class="left-container display-none padding-none display-block-tablet width-full"
			data-toc-container="true"
		>
			<div
				id="ms--toc-content"
				class="padding-left-sm padding-right-none padding-bottom-sm height-full"
			>
				<nav
					id="affixed-left-container"
					class="margin-top-xxs-tablet position-sticky display-flex flex-direction-column width-full"
					aria-label="Primary"
					data-bi-name="left-toc"
					role="navigation"
				>
					<div
						id="ms--collapsible-toc-header"
						class="display-flex flex-direction-row-reverse justify-content-center align-items-center margin-bottom-xxs margin-right-xxs"
					>
						<button
							type="button"
							class="button button-clear inner-focus"
							data-collapsible-toc-toggle
							aria-expanded="true"
							aria-controls="ms--toc-content"
							aria-label="Table of contents"
						>
							<span class="icon icon-mirrored-rtl font-size-md" aria-hidden="true">
								<span class="docon docon-panel-left-contract"></span>
							</span>
						</button>
						<div id="ms--collapsible-toc-moniker-slot" class="flex-grow-1"></div>
					</div>
				</nav>
			</div>
		</div>
	
			  </section>

				<main
					id="main"
					role="main"
					class="layout-body-main "
					data-bi-name="content"
					lang="en-us"
					dir="ltr"
				>
					
			<div
		id="ms--content-header"
		class="content-header default-focus border-bottom-none"
		data-bi-name="content-header"
	>
		<div class="content-header-controls margin-xxs margin-inline-sm-tablet">
			<button
				type="button"
				class="contents-button button button-sm margin-right-xxs"
				data-bi-name="contents-expand"
				aria-haspopup="true"
				data-contents-button
			>
				<span class="icon" aria-hidden="true"><span class="docon docon-menu"></span></span>
				<span class="contents-expand-title"> Table of contents </span>
			</button>
			<button
				type="button"
				class="ap-collapse-behavior ap-expanded button button-sm"
				data-bi-name="ap-collapse"
				aria-controls="action-panel"
			>
				<span class="icon" aria-hidden="true"><span class="docon docon-exit-mode"></span></span>
				<span>Exit editor mode</span>
			</button>
		</div>
	</div>
			<div
				data-main-column
				class="reading-width margin-inline-auto layout-padding padding-top-none padding-top-sm-tablet padding-bottom-sm"
			>
				<div>
					
		<div id="article-header" class="background-color-body margin-bottom-xs display-none-print">
			<div class="display-flex align-items-center justify-content-space-between">
				
		<details
			id="article-header-breadcrumbs-overflow-popover"
			class="popover popover-left"
			data-for="article-header-breadcrumbs"
		>
			<summary
				class="button button-clear button-primary button-sm inner-focus"
				aria-label="All breadcrumbs"
			>
				<span class="icon" aria-hidden="true">
					<span class="docon docon-more"></span>
				</span>
			</summary>
			<div id="article-header-breadcrumbs-overflow" class="popover-content"></div>
		</details>

		<bread-crumbs
			id="article-header-breadcrumbs"
			role="group"
			aria-label="Breadcrumbs"
			data-test-id="article-header-breadcrumbs"
			class="overflow-hidden flex-grow-1 margin-right-sm margin-right-md-tablet margin-right-lg-desktop margin-left-negative-xxs padding-left-xxs"
		></bread-crumbs>
	 
		<div
			id="article-header-page-actions"
			class="opacity-none margin-left-auto display-flex flex-wrap-no-wrap align-items-stretch"
		>
			
		<button
			class="button button-sm border-none inner-focus display-none-tablet flex-shrink-0 "
			data-bi-name="ask-learn-assistant-entry"
			data-test-id="ask-learn-assistant-modal-entry-mobile"
			data-ask-learn-modal-entry
			
			type="button"
			style="min-width: max-content;"
			aria-expanded="false"
			aria-label="Ask Learn"
			hidden
		>
			<span class="icon font-size-lg" aria-hidden="true">
				<span class="docon docon-chat-sparkle-fill gradient-ask-learn-logo"></span>
			</span>
		</button>
		<button
			class="button button-sm display-none display-inline-flex-tablet display-none-desktop flex-shrink-0 margin-right-xxs border-color-ask-learn "
			data-bi-name="ask-learn-assistant-entry"
			
			data-test-id="ask-learn-assistant-modal-entry-tablet"
			data-ask-learn-modal-entry
			type="button"
			style="min-width: max-content;"
			aria-expanded="false"
			hidden
		>
			<span class="icon font-size-lg" aria-hidden="true">
				<span class="docon docon-chat-sparkle-fill gradient-ask-learn-logo"></span>
			</span>
			<span>Ask Learn</span>
		</button>
		<button
			class="button button-sm display-none flex-shrink-0 display-inline-flex-desktop margin-right-xxs border-color-ask-learn "
			data-bi-name="ask-learn-assistant-entry"
			
			data-test-id="ask-learn-assistant-flyout-entry"
			data-ask-learn-flyout-entry
			data-flyout-button="toggle"
			type="button"
			style="min-width: max-content;"
			aria-expanded="false"
			aria-controls="ask-learn-flyout"
			hidden
		>
			<span class="icon font-size-lg" aria-hidden="true">
				<span class="docon docon-chat-sparkle-fill gradient-ask-learn-logo"></span>
			</span>
			<span>Ask Learn</span>
		</button>
	 

			<details class="popover popover-right" id="article-header-page-actions-overflow">
				<summary
					class="justify-content-flex-start button button-clear button-sm button-primary inner-focus"
					aria-label="More actions"
					title="More actions"
				>
					<span class="icon" aria-hidden="true">
						<span class="docon docon-more-vertical"></span>
					</span>
				</summary>
				<div class="popover-content">
					
		<button
			type="button"
			id="ms--focus-mode-button"
			data-focus-mode
			data-bi-name="focus-mode-entry"
			data-page-action-item="overflow-all"
			data-popover-close
			class="button button-clear button-sm button-block justify-content-flex-start text-align-left inner-focus display-none display-inline-flex-tablet"
		>
			<span class="icon" aria-hidden="true">
				<span class="docon docon-glasses"></span>
			</span>
			<span>Reading mode</span>
		</button>
	 
		<button
			data-page-action-item="overflow-mobile"
			type="button"
			class="button-block button-sm inner-focus button button-clear display-none-tablet justify-content-flex-start text-align-left"
			data-bi-name="contents-expand"
			data-contents-button
			data-popover-close
		>
			<span class="icon" aria-hidden="true"
				><span class="docon docon-editor-list-bullet"></span
			></span>
			<span class="contents-expand-title">Table of contents</span>
		</button>
	 
		<a
			id="lang-link-overflow"
			class="button-sm inner-focus button button-clear button-block justify-content-flex-start text-align-left"
			data-bi-name="language-toggle"
			data-page-action-item="overflow-all"
			data-check-hidden="true"
			data-read-in-link
			href="#"
			hidden
		>
			<span class="icon" aria-hidden="true" data-read-in-link-icon>
				<span class="docon docon-locale-globe"></span>
			</span>
			<span data-read-in-link-text>Read in English</span>
		</a>
	
					
		<button
			type="button"
			class="collection button button-clear button-sm button-block justify-content-flex-start text-align-left inner-focus"
			data-list-type="collection"
			data-bi-name="collection"
			data-page-action-item="overflow-all"
			data-check-hidden="true"
			data-popover-close
		>
			<span class="icon" aria-hidden="true">
				<span class="docon docon-circle-addition"></span>
			</span>
			<span class="collection-status">Add</span>
		</button>
	 
		<button
			type="button"
			class="collection button button-block button-clear button-sm justify-content-flex-start text-align-left inner-focus"
			data-list-type="plan"
			data-bi-name="plan"
			data-page-action-item="overflow-all"
			data-check-hidden="true"
			data-popover-close
			hidden
		>
			<span class="icon" aria-hidden="true">
				<span class="docon docon-circle-addition"></span>
			</span>
			<span class="plan-status">Add to plan</span>
		</button>
	 
					
		<a
			data-contenteditbtn
			class="button button-clear button-block button-sm inner-focus justify-content-flex-start text-align-left text-decoration-none"
			data-bi-name="edit"
			data-page-action-item="overflow-all"
			data-check-hidden="true"
			
			href="https://github.com/MicrosoftDocs/azure-databases-docs/blob/main/articles/mysql/flexible-server/concepts-server-parameters.md"
			data-original_content_git_url="https://github.com/MicrosoftDocs/azure-databases-docs-pr/blob/live/articles/mysql/flexible-server/concepts-server-parameters.md"
			data-original_content_git_url_template="{repo}/blob/{branch}/articles/mysql/flexible-server/concepts-server-parameters.md"
			data-pr_repo=""
			data-pr_branch=""
		>
			<span class="icon" aria-hidden="true">
				<span class="docon docon-edit-outline"></span>
			</span>
			<span>Edit</span>
		</a>
	  
		<hr class="margin-block-xxs" />
		
				<button
					class="button button-block button-clear button-sm justify-content-flex-start text-align-left inner-focus"
					type="button"
					data-bi-name="copy-markdown"
					data-page-action-item="overflow-all"
					data-copy-markdown
					data-copy-state="idle"
					data-check-hidden="true"
				>
					<span class="icon color-primary" aria-hidden="true">
						<span data-show-when="idle" class="docon docon-code-lang"></span>
						<span data-show-when="loading" class="loader" hidden></span>
						<span data-show-when="success" class="docon docon-check-mark" hidden></span>
					</span>
					<span>Copy Markdown</span>
				</button>
		   
				<button
					class="button button-block button-clear button-sm justify-content-flex-start text-align-left inner-focus"
					type="button"
					data-bi-name="print"
					data-page-action-item="overflow-all"
					data-popover-close
					data-print-page
					data-check-hidden="true"
				>
					<span class="icon color-primary" aria-hidden="true">
						<span class="docon docon-print"></span>
					</span>
					<span>Print</span>
				</button>
		  
	
				</div>
			</details>
		</div>
	
			</div>
		</div>
	  
		<!-- privateUnauthorizedTemplate is hidden by default -->
		<div unauthorized-private-section data-bi-name="permission-content-unauthorized-private" hidden>
			<hr class="hr margin-top-xs margin-bottom-sm" />
			<div class="notification notification-info">
				<div class="notification-content">
					<p class="margin-top-none notification-title">
						<span class="icon" aria-hidden="true"
							><span class="docon docon-exclamation-circle-solid"></span
						></span>
						<span>Note</span>
					</p>
					<p class="margin-top-none authentication-determined not-authenticated">
						Access to this page requires authorization. You can try <a class="docs-sign-in" href="#" data-bi-name="permission-content-sign-in">signing in</a> or <a  class="docs-change-directory" data-bi-name="permisson-content-change-directory">changing directories</a>.
					</p>
					<p class="margin-top-none authentication-determined authenticated">
						Access to this page requires authorization. You can try <a class="docs-change-directory" data-bi-name="permisson-content-change-directory">changing directories</a>.
					</p>
				</div>
			</div>
		</div>
	
					<div class="content"><h1 id="server-parameters-in-azure-database-for-mysql---flexible-server">Server parameters in Azure Database for MySQL - Flexible Server</h1></div>
					
		<div
			id="article-metadata"
			data-bi-name="article-metadata"
			data-test-id="article-metadata"
			class="page-metadata-container display-flex gap-xxs justify-content-space-between align-items-center flex-wrap-wrap"
		>
			 
				<div
					id="user-feedback"
					class="margin-block-xxs display-none display-none-print"
					hidden
					data-hide-on-archived
				>
					
		<button
			id="user-feedback-button"
			data-test-id="conceptual-feedback-button"
			class="button button-sm button-clear button-primary display-none"
			type="button"
			data-bi-name="user-feedback-button"
			data-user-feedback-button
			hidden
		>
			<span class="icon" aria-hidden="true">
				<span class="docon docon-like"></span>
			</span>
			<span>Feedback</span>
		</button>
	
				</div>
		  
		</div>
	 
		<div data-id="ai-summary" class="display-none-print">
			<div id="ms--ai-summary-cta" class="margin-top-xs display-flex align-items-center">
				<span class="icon" aria-hidden="true">
					<span class="docon docon-sparkle-fill gradient-text-vivid"></span>
				</span>
				<button
					id="ms--ai-summary"
					type="button"
					class="tag tag-sm tag-suggestion margin-left-xxs"
					data-test-id="ai-summary-cta"
					data-bi-name="ai-summary-cta"
					data-an="ai-summary"
				>
					<span class="ai-summary-cta-text">
						Summarize this article for me
					</span>
				</button>
			</div>
			<!-- Slot where the client will render the summary card after the user clicks the CTA -->
			<div id="ms--ai-summary-header" class="margin-top-xs"></div>
		</div>
	 
		<nav
			id="center-doc-outline"
			class="doc-outline display-none-desktop display-none-print margin-bottom-sm"
			data-bi-name="intopic toc"
			aria-label="In this article"
		>
			<h2 id="ms--in-this-article" class="title is-6 margin-block-xs">
				In this article
			</h2>
		</nav>
	
					<div class="content"><p>This article provides considerations and guidelines for configuring server parameters in Azure Database for MySQL - Flexible Server.</p>
<div class="NOTE">
<p>Note</p>
<p>This article contains references to the term <em>slave</em>, which Microsoft no longer uses. When the term is removed from the software, we'll remove it from this article.</p>
</div>
<h2 id="what-are-server-parameters">What are server parameters?</h2>
<p>The MySQL engine provides many <a href="https://dev.mysql.com/doc/refman/5.7/en/server-option-variable-reference.html" data-linktype="external">server parameters</a> (also called <em>variables</em>) that you can use to configure and tune engine behavior. Some parameters can be set dynamically during runtime. Others are static and require a server restart after you set them.</p>
<p>In Azure Database for MySQL - Flexible Server, you can change the value of various MySQL server parameters by using the <a href="how-to-configure-server-parameters-portal" data-linktype="relative-path">Configure server parameters in Azure Database for MySQL - Flexible Server using the Azure portal</a> and the <a href="how-to-configure-server-parameters-cli" data-linktype="relative-path">Configure server parameters in Azure Database for MySQL - Flexible Server using the Azure CLI</a> to match your workload's needs.</p>
<h2 id="configurable-server-parameters">Configurable server parameters</h2>
<p>You can manage the configuration of an Azure Database for MySQL Flexible Server by using server parameters. The server parameters are configured with the default and recommended values when you create the server. The <strong>Server parameters</strong> pane in the Azure portal shows both the modifiable and nonmodifiable parameters. The nonmodifiable server parameters are unavailable.</p>
<p>The list of supported server parameters is constantly growing. You can use the Azure portal to periodically view the full list of server parameters and configure the values.</p>
<p>If you modify a static server parameter by using the portal, you need to restart the server for the changes to take effect. If you're using automation scripts (through tools like Azure Resource Manager templates, Terraform, or the Azure CLI), your script should have a provision to restart the service for the settings to take effect, even if you're changing the configuration as a part of the creation experience.</p>
<p>If you want to modify a nonmodifiable server parameter for your environment, <a href="https://feedback.azure.com/d365community/forum/47b1e71d-ee24-ec11-b6e6-000d3a4f0da0" data-linktype="external">post an idea via community feedback</a>, or vote if the feedback already exists (which can help us prioritize).</p>
<p>The following sections describe the limits of the commonly updated server parameters. The compute tier and the size (vCores) of the server determine the limits.</p>
<h3 id="lower_case_table_names">lower_case_table_names</h3>
<p>For <a href="https://dev.mysql.com/doc/refman/8.0/en/identifier-case-sensitivity.html" data-linktype="external">MySQL version 8.0+</a> you can configure <code>lower_case_table_names</code> only when you're initializing the server. <a href="https://dev.mysql.com/doc/refman/8.0/en/identifier-case-sensitivity.html" data-linktype="external">Learn more</a>. Changing the <code>lower_case_table_names</code> setting after the server is initialized is prohibited. Supported values for MySQL version 8.0 are <code>1</code> and <code>2</code> in Azure Database for MySQL - Flexible Server. The default value is <code>1</code>.</p>
<p>You can configure these settings in the portal during server creation by specifying the desired value under Server Parameters on the Additional Configuration page. For restore operations or replica server creation, the parameter will automatically be copied from the source server and cannot be changed.</p>
<p><span class="mx-imgBorder">
<a href="media/concepts-server-parameters/flexible-server-lower-case-configure.png#lightbox" data-linktype="relative-path">
<img src="media/concepts-server-parameters/flexible-server-lower-case-configure.png" alt="Screenshot that shows how to configure lower case table name server parameter at the time of creation." data-linktype="relative-path">
</a>
</span>
</p>
<p>For MySQL version 5.7, the default value of <code>lower_case_table_names</code> is <code>1</code> in Azure Database for MySQL - Flexible Server. Although it's possible to change the supported value to <code>2</code>, reverting from <code>2</code> back to <code>1</code> isn't allowed. For assistance in changing the default value, <a href="https://azure.microsoft.com/support/create-ticket/" data-linktype="external">create a support ticket</a>.</p>
<h3 id="innodb_tmpdir">innodb_tmpdir</h3>
<p>You use the <a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_tmpdir" data-linktype="external">innodb_tmpdir</a> parameter in Azure Database for MySQL - Flexible Server to define the directory for temporary sort files created during online <code>ALTER TABLE</code> operations that rebuild.</p>
<p>The default value of <code>innodb_tmpdir</code> is <code>/mnt/temp</code>. This location corresponds to the <a href="concepts-service-tiers-storage#service-tiers-size-and-server-types" data-linktype="relative-path">temporary storage (SSD)</a> and is available in gibibytes (GiB) with each server compute size. This location is ideal for operations that don't require a large amount of space.</p>
<p>If you need more space, you can set <code>innodb_tmpdir</code> to <code>/app/work/tmpdir</code>. This setting utilizes the available storage capacity on your Azure Database for MySQL Flexible Server. This setting can be useful for larger operations that require more temporary storage.</p>
<p>Keep in mind that using <code>/app/work/tmpdir</code> results in slower performance compared to the <a href="concepts-service-tiers-storage#service-tiers-size-and-server-types" data-linktype="relative-path">default temporary storage (SSD)</a> <code>/mnt/temp</code> value. Make the choice based on the specific requirements of the operations.</p>
<p>The information provided for <code>innodb_tmpdir</code> is applicable to the parameters <a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_temp_tablespaces_dir" data-linktype="external">innodb_temp_tablespaces_dir</a>, <a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_tmpdir" data-linktype="external">tmpdir</a>, and <a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_load_tmpdir" data-linktype="external">slave_load_tmpdir</a> where:</p>
<ul>
<li>The default value <code>/mnt/temp</code> is common.</li>
<li>The alternative directory <code>/app/work/tmpdir</code> is available for configuring increased temporary storage, with a trade-off in performance based on specific operational requirements.</li>
</ul>
<h3 id="log_bin_trust_function_creators">log_bin_trust_function_creators</h3>
<p>In Azure Database for MySQL - Flexible Server, binary logs are always enabled (that is, <code>log_bin</code> is set to <code>ON</code>). The <a href="https://dev.mysql.com/doc/refman/5.7/en/replication-options-binary-log.html#sysvar_log_bin_trust_function_creators" data-linktype="external"><code>log_bin_trust_function_creators</code></a> parameter is set to <code>ON</code> by default in flexible servers.</p>
<p>The binary logging format is always <code>ROW</code>, and connections to the server always use row-based binary logging. With row-based binary logging, security issues don't exist and binary logging can't break, so you can safely allow <code>log_bin_trust_function_creators</code> to remain as <code>ON</code>.</p>
<p>If <code>log_bin_trust_function_creators</code> is set to <code>OFF</code> and you try to create triggers, you might get errors similar to: "You don't have the SUPER privilege, and binary logging is enabled (you might want to use the less safe <code>log_bin_trust_function_creators</code> variable)."</p>
<h3 id="innodb_buffer_pool_size">innodb_buffer_pool_size</h3>
<p>To learn about the <code>innodb_buffer_pool_size</code> parameter, review the <a href="https://dev.mysql.com/doc/refman/5.7/en/innodb-parameters.html#sysvar_innodb_buffer_pool_size" data-linktype="external">MySQL documentation</a>.</p>
<p>The <a href="concepts-service-tiers-storage#physical-memory-size-gb" data-linktype="relative-path">physical memory size</a> in the following table represents the available random-access memory (RAM), in gigabytes (GB), on your Azure Database for MySQL Flexible Server.</p>
<table>
<thead>
<tr>
<th>Compute size</th>
<th>vCores</th>
<th>Physical memory size (GB)</th>
<th>Default value (bytes)</th>
<th>Min value (bytes)</th>
<th>Max value (bytes)</th>
</tr>
</thead>
<tbody>
<tr>
<td><strong>Burstable</strong></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
</tr>
<tr>
<td>Standard_B1s</td>
<td>1</td>
<td>1</td>
<td>134217728</td>
<td>33554432</td>
<td>268435456</td>
</tr>
<tr>
<td>Standard_B1ms</td>
<td>1</td>
<td>2</td>
<td>536870912</td>
<td>134217728</td>
<td>1073741824</td>
</tr>
<tr>
<td>Standard_B2s</td>
<td>2</td>
<td>4</td>
<td>2147483648</td>
<td>134217728</td>
<td>2147483648</td>
</tr>
<tr>
<td>Standard_B2ms</td>
<td>2</td>
<td>8</td>
<td>4294967296</td>
<td>134217728</td>
<td>5368709120</td>
</tr>
<tr>
<td>Standard_B4ms</td>
<td>4</td>
<td>16</td>
<td>12884901888</td>
<td>134217728</td>
<td>12884901888</td>
</tr>
<tr>
<td>Standard_B8ms</td>
<td>8</td>
<td>32</td>
<td>25769803776</td>
<td>134217728</td>
<td>25769803776</td>
</tr>
<tr>
<td>Standard_B12ms</td>
<td>12</td>
<td>48</td>
<td>51539607552</td>
<td>134217728</td>
<td>32212254720</td>
</tr>
<tr>
<td>Standard_B16ms</td>
<td>16</td>
<td>64</td>
<td>2147483648</td>
<td>134217728</td>
<td>51539607552</td>
</tr>
<tr>
<td>Standard_B20ms</td>
<td>20</td>
<td>80</td>
<td>64424509440</td>
<td>134217728</td>
<td>64424509440</td>
</tr>
<tr>
<td><strong>General Purpose</strong></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
</tr>
<tr>
<td>Standard_D2ads_v5</td>
<td>2</td>
<td>8</td>
<td>4294967296</td>
<td>134217728</td>
<td>5368709120</td>
</tr>
<tr>
<td>Standard_D2ds_v4</td>
<td>2</td>
<td>8</td>
<td>4294967296</td>
<td>134217728</td>
<td>5368709120</td>
</tr>
<tr>
<td>Standard_D4ads_v5</td>
<td>4</td>
<td>16</td>
<td>12884901888</td>
<td>134217728</td>
<td>12884901888</td>
</tr>
<tr>
<td>Standard_D4ds_v4</td>
<td>4</td>
<td>16</td>
<td>12884901888</td>
<td>134217728</td>
<td>12884901888</td>
</tr>
<tr>
<td>Standard_D8ads_v5</td>
<td>8</td>
<td>32</td>
<td>25769803776</td>
<td>134217728</td>
<td>25769803776</td>
</tr>
<tr>
<td>Standard_D8ds_v4</td>
<td>8</td>
<td>32</td>
<td>25769803776</td>
<td>134217728</td>
<td>25769803776</td>
</tr>
<tr>
<td>Standard_D16ads_v5</td>
<td>16</td>
<td>64</td>
<td>51539607552</td>
<td>134217728</td>
<td>51539607552</td>
</tr>
<tr>
<td>Standard_D16ds_v4</td>
<td>16</td>
<td>64</td>
<td>51539607552</td>
<td>134217728</td>
<td>51539607552</td>
</tr>
<tr>
<td>Standard_D32ads_v5</td>
<td>32</td>
<td>128</td>
<td>103079215104</td>
<td>134217728</td>
<td>103079215104</td>
</tr>
<tr>
<td>Standard_D32ds_v4</td>
<td>32</td>
<td>128</td>
<td>103079215104</td>
<td>134217728</td>
<td>103079215104</td>
</tr>
<tr>
<td>Standard_D48ads_v5</td>
<td>48</td>
<td>192</td>
<td>154618822656</td>
<td>134217728</td>
<td>154618822656</td>
</tr>
<tr>
<td>Standard_D48ds_v4</td>
<td>48</td>
<td>192</td>
<td>154618822656</td>
<td>134217728</td>
<td>154618822656</td>
</tr>
<tr>
<td>Standard_D64ads_v5</td>
<td>64</td>
<td>256</td>
<td>206158430208</td>
<td>134217728</td>
<td>206158430208</td>
</tr>
<tr>
<td>Standard_D64ds_v4</td>
<td>64</td>
<td>256</td>
<td>206158430208</td>
<td>134217728</td>
<td>206158430208</td>
</tr>
<tr>
<td><strong>Memory-Optimized</strong></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
</tr>
<tr>
<td>Standard_E2ds_v4</td>
<td>2</td>
<td>16</td>
<td>12884901888</td>
<td>134217728</td>
<td>12884901888</td>
</tr>
<tr>
<td>Standard_E2ads_v5, Standard_E2ds_v5</td>
<td>2</td>
<td>16</td>
<td>12884901888</td>
<td>134217728</td>
<td>12884901888</td>
</tr>
<tr>
<td>Standard_E4ds_v4</td>
<td>4</td>
<td>32</td>
<td>25769803776</td>
<td>134217728</td>
<td>25769803776</td>
</tr>
<tr>
<td>Standard_E4ads_v5, Standard_E4ds_v5</td>
<td>4</td>
<td>32</td>
<td>25769803776</td>
<td>134217728</td>
<td>25769803776</td>
</tr>
<tr>
<td>Standard_E8ds_v4</td>
<td>8</td>
<td>64</td>
<td>51539607552</td>
<td>134217728</td>
<td>51539607552</td>
</tr>
<tr>
<td>Standard_E8ads_v5, Standard_E8ds_v5</td>
<td>8</td>
<td>64</td>
<td>51539607552</td>
<td>134217728</td>
<td>51539607552</td>
</tr>
<tr>
<td>Standard_E16ds_v4</td>
<td>16</td>
<td>128</td>
<td>103079215104</td>
<td>134217728</td>
<td>103079215104</td>
</tr>
<tr>
<td>Standard_E16ads_v5, Standard_E16ds_v5</td>
<td>16</td>
<td>128</td>
<td>103079215104</td>
<td>134217728</td>
<td>103079215104</td>
</tr>
<tr>
<td>Standard_E20ds_v4</td>
<td>20</td>
<td>160</td>
<td>128849018880</td>
<td>134217728</td>
<td>128849018880</td>
</tr>
<tr>
<td>Standard_E20ads_v5, Standard_E20ds_v5</td>
<td>20</td>
<td>160</td>
<td>128849018880</td>
<td>134217728</td>
<td>128849018880</td>
</tr>
<tr>
<td>Standard_E32ds_v4</td>
<td>32</td>
<td>256</td>
<td>206158430208</td>
<td>134217728</td>
<td>206158430208</td>
</tr>
<tr>
<td>Standard_E32ads_v5, Standard_E32ds_v5</td>
<td>32</td>
<td>256</td>
<td>206158430208</td>
<td>134217728</td>
<td>206158430208</td>
</tr>
<tr>
<td>Standard_E48ds_v4</td>
<td>48</td>
<td>384</td>
<td>309237645312</td>
<td>134217728</td>
<td>309237645312</td>
</tr>
<tr>
<td>Standard_E48ads_v5, Standard_E48ds_v5</td>
<td>48</td>
<td>384</td>
<td>309237645312</td>
<td>134217728</td>
<td>309237645312</td>
</tr>
<tr>
<td>Standard_E64ds_v4</td>
<td>64</td>
<td>504</td>
<td>405874409472</td>
<td>134217728</td>
<td>405874409472</td>
</tr>
<tr>
<td>Standard_E64ads_v5 , Standard_E64ds_v5</td>
<td>64</td>
<td>512</td>
<td>412316860416</td>
<td>134217728</td>
<td>412316860416</td>
</tr>
<tr>
<td>Standard_E80ids_v4</td>
<td>80</td>
<td>504</td>
<td>405874409472</td>
<td>134217728</td>
<td>405874409472</td>
</tr>
<tr>
<td>Standard_E96ds_v5</td>
<td>96</td>
<td>672</td>
<td>541165879296</td>
<td>134217728</td>
<td>541165879296</td>
</tr>
</tbody>
</table>
<h3 id="innodb_file_per_table">innodb_file_per_table</h3>
<p>MySQL stores the InnoDB table in different tablespaces based on the configuration that you provided during the table creation. The <a href="https://dev.mysql.com/doc/refman/5.7/en/innodb-system-tablespace.html" data-linktype="external">system tablespace</a> is the storage area for the InnoDB data dictionary. A <a href="https://dev.mysql.com/doc/refman/5.7/en/innodb-file-per-table-tablespaces.html" data-linktype="external">file-per-table tablespace</a> contains data and indexes for a single InnoDB table, and it's stored in the file system in its own data file. The <a href="https://dev.mysql.com/doc/refman/5.7/en/innodb-parameters.html#sysvar_innodb_file_per_table" data-linktype="external">innodb_file_per_table</a> server parameter controls this behavior.</p>
<p>Setting <code>innodb_file_per_table</code> to <code>OFF</code> causes InnoDB to create tables in the system tablespace. Otherwise, InnoDB creates tables in file-per-table tablespaces.</p>
<p>Azure Database for MySQL - Flexible Server supports a maximum of 8 terabytes (TB) in a single data file. If your database size is larger than 8 TB, you should create the table in the <code>innodb_file_per_table</code> tablespace. If you have a single table size larger than 8 TB, you should use the partition table.</p>
<h3 id="innodb_log_file_size">innodb_log_file_size</h3>
<p>The value of <a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_log_file_size" data-linktype="external">innodb_log_file_size</a> is the size (in bytes) of each <a href="https://dev.mysql.com/doc/refman/8.0/en/glossary.html#glos_log_file" data-linktype="external">log file</a> in a <a href="https://dev.mysql.com/doc/refman/8.0/en/glossary.html#glos_log_group" data-linktype="external">log group</a>. The combined size of log files <a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_log_file_size" data-linktype="external">(innodb_log_file_size</a> * <a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_log_files_in_group" data-linktype="external">innodb_log_files_in_group</a>) can't exceed a maximum value that is slightly less than 512 GB.</p>
<p>A bigger log file size is better for performance, but the drawback is that the recovery time after a crash is high. You need to balance recovery time for the rare event of a crash versus maximizing throughput during peak operations. A bigger log file size can also result in longer restart times.</p>
<p>You can configure <code>innodb_log_size</code> to 256 megabytes (MB), 512 MB, 1 GB, or 2 GB for Azure Database for MySQL - Flexible Server. The parameter is static and requires a restart.</p>
<div class="NOTE">
<p>Note</p>
<p>If you changed the <code>innodb_log_file_size</code> parameter from the default, check if the value of <code>show global status like 'innodb_buffer_pool_pages_dirty'</code> stays at <code>0</code> for 30 seconds to avoid restart delay.</p>
</div>
<h3 id="max_connections">max_connections</h3>
<p>The memory size of the server determines the value of <code>max_connections</code>. The <a href="concepts-service-tiers-storage#physical-memory-size-gb" data-linktype="relative-path">physical memory size</a> in the following table represents the available RAM, in gigabytes, on your Azure Database for MySQL Flexible Server.</p>
<table>
<thead>
<tr>
<th>Compute size</th>
<th>vCores</th>
<th>Physical memory size (GB)</th>
<th>Default value</th>
<th>Min value</th>
<th>Max value</th>
</tr>
</thead>
<tbody>
<tr>
<td><strong>Burstable</strong></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
</tr>
<tr>
<td>Standard_B1s</td>
<td>1</td>
<td>1</td>
<td>85</td>
<td>10</td>
<td>171</td>
</tr>
<tr>
<td>Standard_B1ms</td>
<td>1</td>
<td>2</td>
<td>171</td>
<td>10</td>
<td>341</td>
</tr>
<tr>
<td>Standard_B2s</td>
<td>2</td>
<td>4</td>
<td>341</td>
<td>10</td>
<td>683</td>
</tr>
<tr>
<td>Standard_B2ms</td>
<td>2</td>
<td>4</td>
<td>683</td>
<td>10</td>
<td>1365</td>
</tr>
<tr>
<td>Standard_B4ms</td>
<td>4</td>
<td>16</td>
<td>1365</td>
<td>10</td>
<td>2731</td>
</tr>
<tr>
<td>Standard_B8ms</td>
<td>8</td>
<td>32</td>
<td>2731</td>
<td>10</td>
<td>5461</td>
</tr>
<tr>
<td>Standard_B12ms</td>
<td>12</td>
<td>48</td>
<td>4097</td>
<td>10</td>
<td>8193</td>
</tr>
<tr>
<td>Standard_B16ms</td>
<td>16</td>
<td>64</td>
<td>5461</td>
<td>10</td>
<td>10923</td>
</tr>
<tr>
<td>Standard_B20ms</td>
<td>20</td>
<td>80</td>
<td>6827</td>
<td>10</td>
<td>13653</td>
</tr>
<tr>
<td><strong>General Purpose</strong></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
</tr>
<tr>
<td>Standard_D2ads_v5</td>
<td>2</td>
<td>8</td>
<td>683</td>
<td>10</td>
<td>1365</td>
</tr>
<tr>
<td>Standard_D2ds_v4</td>
<td>2</td>
<td>8</td>
<td>683</td>
<td>10</td>
<td>1365</td>
</tr>
<tr>
<td>Standard_D4ads_v5</td>
<td>4</td>
<td>16</td>
<td>1365</td>
<td>10</td>
<td>2731</td>
</tr>
<tr>
<td>Standard_D4ds_v4</td>
<td>4</td>
<td>16</td>
<td>1365</td>
<td>10</td>
<td>2731</td>
</tr>
<tr>
<td>Standard_D8ads_v5</td>
<td>8</td>
<td>32</td>
<td>2731</td>
<td>10</td>
<td>5461</td>
</tr>
<tr>
<td>Standard_D8ds_v4</td>
<td>8</td>
<td>32</td>
<td>2731</td>
<td>10</td>
<td>5461</td>
</tr>
<tr>
<td>Standard_D16ads_v5</td>
<td>16</td>
<td>64</td>
<td>5461</td>
<td>10</td>
<td>10923</td>
</tr>
<tr>
<td>Standard_D16ds_v4</td>
<td>16</td>
<td>64</td>
<td>5461</td>
<td>10</td>
<td>10923</td>
</tr>
<tr>
<td>Standard_D32ads_v5</td>
<td>32</td>
<td>128</td>
<td>10923</td>
<td>10</td>
<td>21845</td>
</tr>
<tr>
<td>Standard_D32ds_v4</td>
<td>32</td>
<td>128</td>
<td>10923</td>
<td>10</td>
<td>21845</td>
</tr>
<tr>
<td>Standard_D48ads_v5</td>
<td>48</td>
<td>192</td>
<td>16384</td>
<td>10</td>
<td>32768</td>
</tr>
<tr>
<td>Standard_D48ds_v4</td>
<td>48</td>
<td>192</td>
<td>16384</td>
<td>10</td>
<td>32768</td>
</tr>
<tr>
<td>Standard_D64ads_v5</td>
<td>64</td>
<td>256</td>
<td>21845</td>
<td>10</td>
<td>43691</td>
</tr>
<tr>
<td>Standard_D64ds_v4</td>
<td>64</td>
<td>256</td>
<td>21845</td>
<td>10</td>
<td>43691</td>
</tr>
<tr>
<td><strong>Memory-Optimized</strong></td>
<td></td>
<td></td>
<td></td>
<td></td>
<td></td>
</tr>
<tr>
<td>Standard_E2ds_v4</td>
<td>2</td>
<td>16</td>
<td>1365</td>
<td>10</td>
<td>2731</td>
</tr>
<tr>
<td>Standard_E2ads_v5, Standard_E2ds_v5</td>
<td>2</td>
<td>16</td>
<td>1365</td>
<td>10</td>
<td>2731</td>
</tr>
<tr>
<td>Standard_E4ds_v4</td>
<td>4</td>
<td>32</td>
<td>2731</td>
<td>10</td>
<td>5461</td>
</tr>
<tr>
<td>Standard_E4ads_v5, Standard_E4ds_v5</td>
<td>4</td>
<td>32</td>
<td>2731</td>
<td>10</td>
<td>5461</td>
</tr>
<tr>
<td>Standard_E8ds_v4</td>
<td>8</td>
<td>64</td>
<td>5461</td>
<td>10</td>
<td>10923</td>
</tr>
<tr>
<td>Standard_E8ads_v5, Standard_E8ds_v5</td>
<td>8</td>
<td>64</td>
<td>5461</td>
<td>10</td>
<td>10923</td>
</tr>
<tr>
<td>Standard_E16ds_v4</td>
<td>16</td>
<td>128</td>
<td>10923</td>
<td>10</td>
<td>21845</td>
</tr>
<tr>
<td>Standard_E16ads_v5, Standard_E16ds_v5</td>
<td>16</td>
<td>128</td>
<td>10923</td>
<td>10</td>
<td>21845</td>
</tr>
<tr>
<td>Standard_E20ds_v4</td>
<td>20</td>
<td>160</td>
<td>13653</td>
<td>10</td>
<td>27306</td>
</tr>
<tr>
<td>Standard_E20ads_v5, Standard_E20ds_v5</td>
<td>20</td>
<td>160</td>
<td>13653</td>
<td>10</td>
<td>27306</td>
</tr>
<tr>
<td>Standard_E32ds_v4</td>
<td>32</td>
<td>256</td>
<td>21845</td>
<td>10</td>
<td>43691</td>
</tr>
<tr>
<td>Standard_E32ads_v5, Standard_E32ds_v5</td>
<td>32</td>
<td>256</td>
<td>21845</td>
<td>10</td>
<td>43691</td>
</tr>
<tr>
<td>Standard_E48ds_v4</td>
<td>48</td>
<td>384</td>
<td>32768</td>
<td>10</td>
<td>65536</td>
</tr>
<tr>
<td>Standard_E48ads_v5, Standard_E48ds_v5</td>
<td>48</td>
<td>384</td>
<td>32768</td>
<td>10</td>
<td>65536</td>
</tr>
<tr>
<td>Standard_E64ds_v4</td>
<td>64</td>
<td>504</td>
<td>43008</td>
<td>10</td>
<td>86016</td>
</tr>
<tr>
<td>Standard_E64ads_v5, Standard_E64ds_v5</td>
<td>64</td>
<td>512</td>
<td>43691</td>
<td>10</td>
<td>87383</td>
</tr>
<tr>
<td>Standard_E80ids_v4</td>
<td>80</td>
<td>504</td>
<td>43008</td>
<td>10</td>
<td>86016</td>
</tr>
<tr>
<td>Standard_E96ds_v5</td>
<td>96</td>
<td>672</td>
<td>50000</td>
<td>10</td>
<td>100000</td>
</tr>
</tbody>
</table>
<p>When connections exceed the limit, you might receive the following error: "ERROR 1040 (08004): Too many connections."</p>
<p>Creating new client connections to MySQL takes time. After you establish these connections, they occupy database resources, even when they're idle. Most applications request many short-lived connections, which compounds this situation. The result is fewer resources available for your actual workload, leading to decreased performance.</p>
<p>A connection pooler that decreases idle connections and reuses existing connections helps you avoid this problem. For the best experience, we recommend that you use a connection pooler like ProxySQL to efficiently manage connections. To learn about setting up ProxySQL, see <a href="https://techcommunity.microsoft.com/blog/adformysql/load-balance-read-replicas-using-proxysql-in-azure-database-for-mysql/880042" data-linktype="external">this blog post</a>.</p>
<div class="NOTE">
<p>Note</p>
<p>ProxySQL is an open-source community tool. Microsoft supports it on a best-effort basis. To get production support with authoritative guidance, contact <a href="https://proxysql.com/contact-us" data-linktype="external">ProxySQL product support</a>.</p>
</div>
<h3 id="innodb_strict_mode">innodb_strict_mode</h3>
<p>If you receive an error similar to "Row size too large (&gt; 8126)," you might want to turn off the <code>innodb_strict_mode</code> server parameter. This parameter can't be modified globally at the server level because if row data size is larger than 8K, the data is truncated without an error. This truncation can lead to potential data loss. We recommend modifying the schema to fit the page size limit.</p>
<p>You can set this parameter at the session level by using <code>init_connect</code>. For more information, see <a href="how-to-configure-server-parameters-portal#setting-non-modifiable-server-parameters" data-linktype="relative-path">Setting nonmodifiable server parameters</a>.</p>
<div class="NOTE">
<p>Note</p>
<p>If you have a read replica server, setting <code>innodb_strict_mode</code> to <code>OFF</code> at the session level on a source server will break the replication. We suggest keeping the parameter set to <code>ON</code> if you have read replicas.</p>
</div>
<h3 id="time_zone">time_zone</h3>
<p>You can populate the time zone tables with the latest time zone information by calling the <code>mysql.az_load_timezone</code> stored procedure from a tool like the MySQL command line or MySQL Workbench and then set the global time zones by using the <a href="how-to-configure-server-parameters-portal#working-with-the-time-zone-parameter" data-linktype="relative-path">Azure portal</a> or the <a href="how-to-configure-server-parameters-cli#working-with-the-time-zone-parameter" data-linktype="relative-path">Azure CLI</a>. Time zones are automatically loaded during server creation, removing the need for customers to manually execute the <code>mysql.az_load_timezone</code> stored procedure afterwards to load the time zone.</p>
<h3 id="innodb_temp_data_file_size_max">innodb_temp_data_file_size_max</h3>
<p>For Azure Database for MySQL Flexible Server (version 5.7 only), innodb_temp_data_file_size_max parameter defines the maximum size of InnoDB temporary tablespace data files in MB. Setting the value to 0 means no limit, allowing growth up to the full storage size. Any non-zero value below 64 MB is rounded up to 64 MB, while values above 64 MB are applied as specified. This is a static variable and requires a server restart for changes to take effect.</p>
<div class="NOTE">
<p>Note</p>
<ul>
<li>Note: In MySQL 8.0 and above, the <a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-temporary-tablespace.html" data-linktype="external">global temporary tablespace</a> (ibtmp1) only stores rollback segments for changes made to user-created temporary tables. Therefore, this parameter is no longer relevant.</li>
</ul>
</div>
<h3 id="binlog_expire_logs_seconds">binlog_expire_logs_seconds</h3>
<p>In Azure Database for MySQL - Flexible Server, the <code>binlog_expire_logs_seconds</code> parameter specifies the number of seconds that the service waits before deleting the binary log file.</p>
<p>The binary log contains events that describe database changes, such as table creation operations or changes to table data. The binary log also contains events for statements that potentially could have made changes. The binary log is used mainly for two purposes: replication and data recovery operations.</p>
<p>Usually, the binary logs are deleted as soon as the handle is free from the service, backup, or replica set. If there are multiple replicas, the binary logs wait for the slowest replica to read the changes before they're deleted.</p>
<p>If you want to persist binary logs for a longer duration, you can configure the <code>binlog_expire_logs_seconds</code> parameter. If <code>binlog_expire_logs_seconds</code> is set to the default value of <code>0</code>, a binary log is deleted as soon as the handle to it's freed. If the value of <code>binlog_expire_logs_seconds</code> is greater than <code>0</code>, the binary log is deleted after the configured number of seconds.</p>
<p>Azure Database for MySQL - Flexible Server handles managed features, like backup and read replica deletion of binary files, internally. When you replicate the data-out from Azure Database for MySQL - Flexible Server, this parameter needs to be set in the primary to avoid deletion of binary logs before the replica reads from the changes in the primary. If you set <code>binlog_expire_logs_seconds</code> to a higher value, the binary logs won't be deleted soon enough. That delay can lead to an increase in the storage billing.</p>
<h4 id="limitations">Limitations</h4>
<p>Once the accelerated logs feature is enabled, the binlog_expire_logs_seconds server parameter is disregarded entirely, and any configured value will no longer have any effect. However, if the accelerated logs feature is disabled, the server will once again adhere to the configured value of binlog_expire_logs_seconds for binary log retention. This applies to replica servers as well.</p>
<h3 id="event_scheduler">event_scheduler</h3>
<p>In Azure Database for MySQL - Flexible Server, the <code>event_scheduler</code> server parameter manages creating, scheduling, and running events. That is, the parameter manages tasks that run according to a schedule by a special MySQL Event Scheduler thread. When the <code>event_scheduler</code> parameter is set to <code>ON</code>, the Event Scheduler thread is listed as a daemon process in the output of <code>SHOW PROCESSLIST</code>.</p>
<p>For MySQL version 5.7 servers, the server parameter <code>event_scheduler</code> is automatically turned 'OFF' when <a href="concepts-backup-restore#backup-overview" data-linktype="relative-path">backup</a> is initiated and server parameter <code>event_scheduler</code> is turned back 'ON' after the backup completes successfully. In MySQL version 8.0 for Azure Database for MySQL - Flexible Server, the event_scheduler remains unaffected during <a href="concepts-backup-restore#backup-overview" data-linktype="relative-path">backups</a>. To ensure smoother operations, it's recommended to upgrade your MySQL 5.7 servers to version 8.0 using a <a href="how-to-upgrade" data-linktype="relative-path">major version upgrade</a>.</p>
<p>You can create and schedule events by using the following SQL syntax:</p>
<pre><code class="lang-sql">CREATE EVENT &lt;event name&gt;
ON SCHEDULE EVERY _ MINUTE / HOUR / DAY
STARTS TIMESTAMP / CURRENT_TIMESTAMP
ENDS TIMESTAMP / CURRENT_TIMESTAMP + INTERVAL 1 MINUTE / HOUR / DAY
COMMENT '&lt;comment&gt;'
DO
&lt;your statement&gt;;
</code></pre>
<p>For more information about creating an event, see the following documentation about the Event Scheduler in the MySQL reference manual:</p>
<ul>
<li><a href="https://dev.mysql.com/doc/refman/5.7/en/event-scheduler.html" data-linktype="external">Using the Event Scheduler in MySQL 5.7</a></li>
<li><a href="https://dev.mysql.com/doc/refman/8.0/en/event-scheduler.html" data-linktype="external">Using the Event Scheduler in MySQL 8.0</a></li>
</ul>
<p><a id="configuring-the-event_scheduler-server-parameter"></a></p>
<h4 id="configure-the-event_scheduler-server-parameter">Configure the event_scheduler server parameter</h4>
<p>The following scenario illustrates one way to use the <code>event_scheduler</code> parameter in Azure Database for MySQL - Flexible Server.</p>
<p>To demonstrate the scenario, consider the following example of a simple table:</p>
<pre><code class="lang-sql">mysql&gt; describe tab1;
+-----------+-------------+------+-----+---------+----------------+
| Field | Type | Null | Key | Default | Extra |
| +-----------+-------------+------+-----+---------+----------------+ |
| id | int(11) | NO | PRI | NULL | auto_increment |
| CreatedAt | timestamp | YES | | NULL | |
| CreatedBy | varchar(16) | YES | | NULL | |
| +-----------+-------------+------+-----+---------+----------------+ |
| 3 rows in set (0.23 sec) |
| ``` |
| To configure the `event_scheduler` server parameter in Azure Database for MySQL - Flexible Server, perform the following steps: |

1. In the Azure portal, go to your Azure Database for MySQL - Flexible Server instance. Under **Settings**, select **Server parameters**.
1. On the **Server parameters** pane, search for `event_scheduler`. In the **VALUE** dropdown list, select **ON**, and then select **Save**.

    &gt; [!NOTE]
    &gt; Deployment of the dynamic configuration change to the server parameter doesn't require a restart.

1. To create an event, connect to the Azure Database for MySQL - Flexible Server instance and run the following SQL command:
    ```sql

    CREATE EVENT test_event_01
    ON SCHEDULE EVERY 1 MINUTE
    STARTS CURRENT_TIMESTAMP
    ENDS CURRENT_TIMESTAMP + INTERVAL 1 HOUR
    COMMENT 'Inserting record into the table tab1 with current timestamp'
    DO
    INSERT INTO tab1(id,createdAt,createdBy)
    VALUES('',NOW(),CURRENT_USER());

    ```
1. To view the Event Scheduler details, run the following SQL statement:
    ```sql

    SHOW EVENTS;

    ```
    The following output appears:
    ```sql

    mysql&gt; show events;
    +-----+---------------+-------------+-----------+-----------+------------+----------------+----------------+---------------------+---------------------+---------+------------+----------------------+----------------------+--------------------+
    | Db | Name | Definer | Time zone | Type | Execute at | Interval value | Interval field | Starts | Ends | Status | Originator | character_set_client | collation_connection | Database Collation |
    | +-----+---------------+-------------+-----------+-----------+------------+----------------+----------------+---------------------+---------------------+---------+------------+----------------------+----------------------+--------------------+ |
    | db1 | test_event_01 | azureuser@% | SYSTEM | RECURRING | NULL | 1 | MINUTE | 2023-04-05 14:47:04 | 2023-04-05 15:47:04 | ENABLED | 3221153808 | latin1 | latin1_swedish_ci | latin1_swedish_ci |
    | +-----+---------------+-------------+-----------+-----------+------------+----------------+----------------+---------------------+---------------------+---------+------------+----------------------+----------------------+--------------------+ |
    | 1 row in set (0.23 sec) |
    | ``` |

1. After a few minutes, query the rows from the table to begin viewing the rows inserted every minute according to the `event_scheduler` parameter that you configured:

    ```azurecli
    mysql&gt; select * from tab1;
    +----+---------------------+-------------+
    | id | CreatedAt | CreatedBy |
    | +----+---------------------+-------------+ |
    | 1 | 2023-04-05 14:47:04 | azureuser@% |
    | 2 | 2023-04-05 14:48:04 | azureuser@% |
    | 3 | 2023-04-05 14:49:04 | azureuser@% |
    | 4 | 2023-04-05 14:50:04 | azureuser@% |
    | +----+---------------------+-------------+ |
    | 4 rows in set (0.23 sec) |
    | ``` |
| 1. After an hour, run a `select` statement on the table to view the complete result of the values inserted into table every minute for an hour (as `event_scheduler` is configured in this case): |
    ```azurecli

    mysql&gt; select * from tab1;
    +----+---------------------+-------------+
    | id | CreatedAt | CreatedBy |
    | +----+---------------------+-------------+ |
    | 1 | 2023-04-05 14:47:04 | azureuser@% |
    | 2 | 2023-04-05 14:48:04 | azureuser@% |
    | 3 | 2023-04-05 14:49:04 | azureuser@% |
    | 4 | 2023-04-05 14:50:04 | azureuser@% |
    | 5 | 2023-04-05 14:51:04 | azureuser@% |
    | 6 | 2023-04-05 14:52:04 | azureuser@% |
    | ..&lt; 50 lines trimmed to compact output &gt;.. |
    | 56 | 2023-04-05 15:42:04 | azureuser@% |
    | 57 | 2023-04-05 15:43:04 | azureuser@% |
    | 58 | 2023-04-05 15:44:04 | azureuser@% |
    | 59 | 2023-04-05 15:45:04 | azureuser@% |
    | 60 | 2023-04-05 15:46:04 | azureuser@% |
    | 61 | 2023-04-05 15:47:04 | azureuser@% |
    | +----+---------------------+-------------+ |
    | 61 rows in set (0.23 sec) |
    | ``` |

#### Other scenarios

You can set up an event based on the requirements of your specific scenario. A few examples of scheduling SQL statements to run at various time intervals follow.

To run a SQL statement now and repeat one time per day with no end:

```sql
CREATE EVENT &lt;event name&gt;
ON SCHEDULE
EVERY 1 DAY
STARTS (TIMESTAMP(CURRENT_DATE) + INTERVAL 1 DAY + INTERVAL 1 HOUR)
COMMENT 'Comment'
DO
&lt;your statement&gt;;
</code></pre>
<p>To run a SQL statement every hour with no end:</p>
<pre><code class="lang-sql">CREATE EVENT &lt;event name&gt;
ON SCHEDULE
EVERY 1 HOUR
COMMENT 'Comment'
DO
&lt;your statement&gt;;
</code></pre>
<p>To run a SQL statement every day with no end:</p>
<pre><code class="lang-sql">CREATE EVENT &lt;event name&gt;
ON SCHEDULE
EVERY 1 DAY
STARTS str_to_date( date_format(now(), '%Y%m%d 0200'), '%Y%m%d %H%i' ) + INTERVAL 1 DAY
COMMENT 'Comment'
DO
&lt;your statement&gt;;
</code></pre>
<h3 id="innodb_ft_user_stopword_table">innodb_ft_user_stopword_table</h3>
<p><code>innodb_ft_user_stopword_table</code> is a server parameter in MySQL that specifies the name of the table containing custom stopwords for InnoDB Full-Text Search. The table must be in the same database as the full-text indexed table, and its first column must be of type <code>VARCHAR</code>. In Azure Database for MySQL - Flexible Server, the default setting of <code>sql_generate_invisible_primary_key=ON</code> causes all tables without an explicit primary key to automatically include an invisible primary key. This behavior conflicts with the requirements for <code>innodb_ft_user_stopword_table</code>, as the invisible primary key becomes the first column of the table, preventing it from functioning as intended during Full-Text Search. To resolve this issue, you must set <code>sql_generate_invisible_primary_key=OFF</code> in the same session before creating the custom stopword table. For example:</p>
<pre><code class="lang-sql">SET sql_generate_invisible_primary_key = OFF;
CREATE TABLE my_stopword_table (
    stopword VARCHAR(50) NOT NULL
);
INSERT INTO my_stopword_table (stopword) VALUES ('and'), ('or'), ('the');
</code></pre>
<p>This ensures the stopword table meets MySQLâs requirements and allows custom stopwords to work properly.</p>
<h2 id="nonmodifiable-server-parameters">Nonmodifiable server parameters</h2>
<p>The <strong>Server parameters</strong> pane in the Azure portal shows both the modifiable and nonmodifiable server parameters. The nonmodifiable server parameters are unavailable. You can configure a nonmodifiable server parameter at the session level by using <code>init_connect</code> in the <a href="how-to-configure-server-parameters-portal#setting-non-modifiable-server-parameters" data-linktype="relative-path">Azure portal</a> or the <a href="how-to-configure-server-parameters-cli#setting-non-modifiable-server-parameters" data-linktype="relative-path">Azure CLI</a>.</p>
<h2 id="azure-mysql-system-variables">Azure mysql system variables</h2>
<h3 id="azure_server_name">azure_server_name</h3>
<p>The <code>azure_server_name</code> variable provides the exact server name of the Azure Database for MySQL - Flexible Server instance. This variable is useful when applications or scripts need to programmatically retrieve the serverâs hostname they are connected to, without relying on external configurations and can be retrieved by running following command inside MySQL.</p>
<pre><code class="lang-sql">mysql&gt; SHOW GLOBAL VARIABLES LIKE 'azure_server_name';
+-------------------+---------+
| Variable_name     | Value   |
+-------------------+---------+
| azure_server_name | myflex  |
+-------------------+---------+
</code></pre>
<p>Note : The <code>azure_server_name</code> consistently returns the original server name you use to connect to the service (e.g., myflex) for both HA-enabled and HA-disabled server</p>
<h3 id="logical_server_name">logical_server_name</h3>
<p>The <code>logical_server_name</code> variable represents the hostname of the instance where Azure Database for MySQL - Flexible Server is running. This variable is useful for identifying the host where the service is currently running, aiding in troubleshooting and failover monitoring. You can retrieve this variable by executing the following command within MySQL.</p>
<pre><code class="lang-sql">mysql&gt; SHOW GLOBAL VARIABLES LIKE 'logical_server_name';
+---------------------+--------------+
| Variable_name       | Value        |
+---------------------+--------------+
| logical_server_name | myflex	     |
+---------------------+--------------+
</code></pre>
<p>Note: For an HA-enabled server, the <code>logical_server_name</code> variable reflects the hostname of the instance acting as the primary server. For a server where HA is disabled, the value of <code>logical_server_name</code> is the same as the <code>azure_server_name</code> variable since there is only a single instance.</p>
<h2 id="related-content">Related content</h2>
<ul>
<li><a href="how-to-configure-server-parameters-portal" data-linktype="relative-path">Configure server parameters in Azure Database for MySQL - Flexible Server using the Azure portal</a></li>
<li><a href="how-to-configure-server-parameters-cli" data-linktype="relative-path">Configure server parameters in Azure Database for MySQL - Flexible Server using the Azure CLI</a></li>
</ul>
</div>
					
		<div
			id="ms--inline-notifications"
			class="margin-block-xs"
			data-bi-name="inline-notification"
		></div>
	 
		<div
			id="assertive-live-region"
			role="alert"
			aria-live="assertive"
			class="visually-hidden"
			aria-relevant="additions"
			aria-atomic="true"
		></div>
		<div
			id="polite-live-region"
			role="status"
			aria-live="polite"
			class="visually-hidden"
			aria-relevant="additions"
			aria-atomic="true"
		></div>
	
					
		<!-- feedback section -->
		<section
			id="site-user-feedback-footer"
			class="font-size-sm margin-top-md display-none-print display-none-desktop"
			data-test-id="site-user-feedback-footer"
			data-bi-name="site-feedback-section"
		>
			<hr class="hr" />
			<h2 id="ms--feedback" class="title is-3">Feedback</h2>
			<div class="display-flex flex-wrap-wrap align-items-center">
				<p class="font-weight-semibold margin-xxs margin-left-none">
					Was this page helpful?
				</p>
				<div class="buttons">
					<button
						class="thumb-rating-button like button button-primary button-sm"
						data-test-id="footer-rating-yes"
						data-binary-rating-response="rating-yes"
						type="button"
						title="This article is helpful"
						data-bi-name="button-rating-yes"
						aria-pressed="false"
					>
						<span class="icon" aria-hidden="true">
							<span class="docon docon-like"></span>
						</span>
						<span>Yes</span>
					</button>
					<button
						class="thumb-rating-button dislike button button-primary button-sm"
						id="standard-rating-no-button"
						hidden
						data-test-id="footer-rating-no"
						data-binary-rating-response="rating-no"
						type="button"
						title="This article is not helpful"
						data-bi-name="button-rating-no"
						aria-pressed="false"
					>
						<span class="icon" aria-hidden="true">
							<span class="docon docon-dislike"></span>
						</span>
						<span>No</span>
					</button>
					<details
						class="popover popover-top"
						id="mobile-help-popover"
						data-test-id="footer-feedback-popover"
					>
						<summary
							class="thumb-rating-button dislike button button-primary button-sm"
							data-test-id="details-footer-rating-no"
							data-binary-rating-response="rating-no"
							title="This article is not helpful"
							data-bi-name="button-rating-no"
							aria-pressed="false"
							data-bi-an="feedback-unhelpful-popover"
						>
							<span class="icon" aria-hidden="true">
								<span class="docon docon-dislike"></span>
							</span>
							<span>No</span>
						</summary>
						<div
							class="popover-content width-200 width-300-tablet"
							role="dialog"
							aria-labelledby="popover-heading"
							aria-describedby="popover-description"
						>
							<p id="popover-heading" class="font-size-lg margin-bottom-xxs font-weight-semibold">
								Need help with this topic?
							</p>
							<p id="popover-description" class="font-size-sm margin-bottom-xs">
								Want to try using Ask Learn to clarify or guide you through this topic?
							</p>
							
		<div class="buttons flex-direction-row flex-wrap justify-content-center gap-xxs">
			<div>
		<button
			class="button button-sm border inner-focus display-none margin-right-xxs"
			data-bi-name="ask-learn-assistant-entry-troubleshoot"
			data-test-id="ask-learn-assistant-modal-entry-mobile-feedback"
			data-ask-learn-modal-entry-feedback
			data-bi-an=feedback-unhelpful-popover
			type="button"
			style="min-width: max-content;"
			aria-expanded="false"
			aria-label="Ask Learn"
			hidden
		>
			<span class="icon font-size-lg" aria-hidden="true">
				<span class="docon docon-chat-sparkle-fill gradient-ask-learn-logo"></span>
			</span>
		</button>
		<button
			class="button button-sm display-inline-flex display-none-desktop flex-shrink-0 margin-right-xxs border-color-ask-learn margin-right-xxs"
			data-bi-name="ask-learn-assistant-entry-troubleshoot"
			data-bi-an=feedback-unhelpful-popover
			data-test-id="ask-learn-assistant-modal-entry-tablet-feedback"
			data-ask-learn-modal-entry-feedback
			type="button"
			style="min-width: max-content;"
			aria-expanded="false"
			hidden
		>
			<span class="icon font-size-lg" aria-hidden="true">
				<span class="docon docon-chat-sparkle-fill gradient-ask-learn-logo"></span>
			</span>
			<span>Ask Learn</span>
		</button>
		<button
			class="button button-sm display-none flex-shrink-0 display-inline-flex-desktop margin-right-xxs border-color-ask-learn margin-right-xxs"
			data-bi-name="ask-learn-assistant-entry-troubleshoot"
			data-bi-an=feedback-unhelpful-popover
			data-test-id="ask-learn-assistant-flyout-entry-feedback"
			data-ask-learn-flyout-entry-show-only
			data-flyout-button="toggle"
			type="button"
			style="min-width: max-content;"
			aria-expanded="false"
			aria-controls="ask-learn-flyout"
			hidden
		>
			<span class="icon font-size-lg" aria-hidden="true">
				<span class="docon docon-chat-sparkle-fill gradient-ask-learn-logo"></span>
			</span>
			<span>Ask Learn</span>
		</button>
	</div>
			<button
				type="button"
				class="button button-sm margin-right-xxs"
				data-help-option="suggest-fix"
				data-bi-name="feedback-suggest"
				data-bi-an="feedback-unhelpful-popover"
				data-test-id="suggest-fix"
			>
				<span class="icon" aria-hidden="true">
					<span class="docon docon-feedback"></span>
				</span>
				<span> Suggest a fix? </span>
			</button>
		</div>
	
						</div>
					</details>
				</div>
			</div>
		</section>
		<!-- end feedback section -->
	
				</div>
				
		<div id="ms--additional-resources-mobile" class="display-none-print">
			<hr class="hr" hidden />
			<h2 id="ms--additional-resources-mobile-heading" class="title is-3" hidden>
				Additional resources
			</h2>
			
		<section
			id="right-rail-recommendations-mobile"
			class=""
			data-bi-name="recommendations"
			hidden
		></section>
	 
		<section
			id="right-rail-training-mobile"
			class=""
			data-bi-name="learning-resource-card"
			hidden
		></section>
	 
		<section
			id="right-rail-events-mobile"
			class=""
			data-bi-name="events-card"
			hidden
		></section>
	
		</div>
	 
		<div
			id="article-metadata-footer"
			data-bi-name="article-metadata-footer"
			data-test-id="article-metadata-footer"
			class="page-metadata-container"
		>
			<hr class="hr" />
			<ul class="metadata page-metadata" data-bi-name="page info" lang="en-us" dir="ltr">
				<li class="visibility-hidden-visual-diff">
			<span class="badge badge-sm text-wrap-pretty">
				<span>Last updated on <local-time format="twoDigitNumeric"
		datetime="2025-11-25T08:00:00.000Z"
		data-article-date-source="calculated"
		class="is-invisible"
	>
		2025-11-25
	</local-time></span>
			</span>
		</li>
			</ul>
		</div>
	
			</div>
			
		<div
			id="action-panel"
			role="region"
			aria-label="Action Panel"
			class="action-panel"
			tabindex="-1"
		></div>
	
		
				</main>
				<aside
					id="layout-body-aside"
					class="layout-body-aside  "
					data-bi-name="aside"
					aria-label="Additional resources"
			  >
					
		<div
			id="ms--additional-resources"
			class="right-container padding-sm display-none display-block-desktop height-full"
			data-bi-name="pageactions"
		>
			<div id="affixed-right-container" data-bi-name="right-column">
				
		<nav
			id="side-doc-outline"
			class="doc-outline border-bottom padding-bottom-xs margin-bottom-xs scrollbar-width-thin"
			data-bi-name="intopic toc"
			aria-label="In this article"
		>
			<h3>In this article</h3>
		</nav>
	
				<!-- Feedback -->
				
		<section
			id="ms--site-user-feedback-right-rail"
			class="font-size-sm display-none-print"
			data-test-id="site-user-feedback-right-rail"
			data-bi-name="site-feedback-right-rail"
		>
			<div class="display-flex flex-wrap-wrap align-items-center">
				<p class="font-weight-semibold margin-xxs margin-left-none">
					Was this page helpful?
				</p>
				<div class="display-flex flex-wrap-nowrap">
					<button
						class="thumb-rating-button like inner-focus"
						data-test-id="right-rail-rating-yes"
						data-binary-rating-response="rating-yes"
						type="button"
						title="This article is helpful"
						aria-label="This article is helpful"
						data-bi-name="button-rating-yes"
						aria-pressed="false"
					>
						<span class="icon" aria-hidden="true">
							<span class="docon docon-like"></span>
						</span>
					</button>
					<button
						class="thumb-rating-button dislike inner-focus"
						id="right-rail-no-button"
						hidden
						data-test-id="right-rail-rating-no"
						data-binary-rating-response="rating-no"
						type="button"
						title="This article is not helpful"
						aria-label="This article is not helpful"
						data-bi-name="button-rating-no"
						aria-pressed="false"
					>
						<span class="icon" aria-hidden="true">
							<span class="docon docon-dislike"></span>
						</span>
					</button>
					<details class="popover popover-right" id="help-popover" data-test-id="feedback-popover">
						<summary
							tabindex="0"
							class="thumb-rating-button dislike inner-focus"
							data-test-id="details-right-rail-rating-no"
							data-binary-rating-response="rating-no"
							title="This article is not helpful"
							aria-label="This article is not helpful"
							data-bi-name="button-rating-no"
							aria-pressed="false"
							data-bi-an="feedback-unhelpful-popover"
						>
							<span class="icon" aria-hidden="true">
								<span class="docon docon-dislike"></span>
							</span>
						</summary>
						<div
							class="popover-content width-200 width-300-tablet"
							role="dialog"
							aria-labelledby="popover-heading-right-rail"
							aria-describedby="popover-description-right-rail"
						>
							<p
								id="popover-heading-right-rail"
								class="font-size-lg margin-bottom-xxs font-weight-semibold"
							>
								Need help with this topic?
							</p>
							<p id="popover-description-right-rail" class="font-size-sm margin-bottom-xs">
								Want to try using Ask Learn to clarify or guide you through this topic?
							</p>
							
		<div class="buttons flex-direction-row flex-wrap justify-content-center gap-xxs">
			<div>
		<button
			class="button button-sm border inner-focus display-none margin-right-xxs"
			data-bi-name="ask-learn-assistant-entry-troubleshoot"
			data-test-id="ask-learn-assistant-modal-entry-mobile-feedback"
			data-ask-learn-modal-entry-feedback
			data-bi-an=feedback-unhelpful-popover
			type="button"
			style="min-width: max-content;"
			aria-expanded="false"
			aria-label="Ask Learn"
			hidden
		>
			<span class="icon font-size-lg" aria-hidden="true">
				<span class="docon docon-chat-sparkle-fill gradient-ask-learn-logo"></span>
			</span>
		</button>
		<button
			class="button button-sm display-inline-flex display-none-desktop flex-shrink-0 margin-right-xxs border-color-ask-learn margin-right-xxs"
			data-bi-name="ask-learn-assistant-entry-troubleshoot"
			data-bi-an=feedback-unhelpful-popover
			data-test-id="ask-learn-assistant-modal-entry-tablet-feedback"
			data-ask-learn-modal-entry-feedback
			type="button"
			style="min-width: max-content;"
			aria-expanded="false"
			hidden
		>
			<span class="icon font-size-lg" aria-hidden="true">
				<span class="docon docon-chat-sparkle-fill gradient-ask-learn-logo"></span>
			</span>
			<span>Ask Learn</span>
		</button>
		<button
			class="button button-sm display-none flex-shrink-0 display-inline-flex-desktop margin-right-xxs border-color-ask-learn margin-right-xxs"
			data-bi-name="ask-learn-assistant-entry-troubleshoot"
			data-bi-an=feedback-unhelpful-popover
			data-test-id="ask-learn-assistant-flyout-entry-feedback"
			data-ask-learn-flyout-entry-show-only
			data-flyout-button="toggle"
			type="button"
			style="min-width: max-content;"
			aria-expanded="false"
			aria-controls="ask-learn-flyout"
			hidden
		>
			<span class="icon font-size-lg" aria-hidden="true">
				<span class="docon docon-chat-sparkle-fill gradient-ask-learn-logo"></span>
			</span>
			<span>Ask Learn</span>
		</button>
	</div>
			<button
				type="button"
				class="button button-sm margin-right-xxs"
				data-help-option="suggest-fix"
				data-bi-name="feedback-suggest"
				data-bi-an="feedback-unhelpful-popover"
				data-test-id="suggest-fix"
			>
				<span class="icon" aria-hidden="true">
					<span class="docon docon-feedback"></span>
				</span>
				<span> Suggest a fix? </span>
			</button>
		</div>
	
						</div>
					</details>
				</div>
			</div>
		</section>
	
			</div>
		</div>
	
			  </aside> <section
					id="layout-body-flyout"
					class="layout-body-flyout "
					data-bi-name="flyout"
			  >
					 <div
	class="height-full border-left background-color-body-medium"
	id="ask-learn-flyout"
></div>
			  </section> <div class="layout-body-footer " data-bi-name="layout-footer">
		<footer
			id="footer"
			data-test-id="footer"
			data-bi-name="footer"
			class="footer-layout padding-xs padding-sm-desktop has-default-focus border-top"
			role="contentinfo"
		>
			<div class="display-flex gap-xs flex-wrap-wrap is-full-height padding-right-lg-desktop">
				
		<a
			data-mscc-ic="false"
			href="#"
			data-bi-name="select-locale"
			class="locale-selector-link flex-shrink-0 button button-sm button-clear external-link-indicator"
			id=""
			title=""
			><span class="icon" aria-hidden="true"
				><span class="docon docon-world"></span></span
			><span class="local-selector-link-text">en-us</span></a
		>
	 <div class="ccpa-privacy-link" data-ccpa-privacy-link hidden>
		
		<a
			data-mscc-ic="false"
			href="https://aka.ms/yourcaliforniaprivacychoices"
			data-bi-name="your-privacy-choices"
			class="button button-sm button-clear flex-shrink-0 external-link-indicator"
			id=""
			title=""
			>
		<svg
			xmlns="http://www.w3.org/2000/svg"
			viewBox="0 0 30 14"
			xml:space="preserve"
			height="16"
			width="43"
			aria-hidden="true"
			focusable="false"
		>
			<path
				d="M7.4 12.8h6.8l3.1-11.6H7.4C4.2 1.2 1.6 3.8 1.6 7s2.6 5.8 5.8 5.8z"
				style="fill-rule:evenodd;clip-rule:evenodd;fill:#fff"
			></path>
			<path
				d="M22.6 0H7.4c-3.9 0-7 3.1-7 7s3.1 7 7 7h15.2c3.9 0 7-3.1 7-7s-3.2-7-7-7zm-21 7c0-3.2 2.6-5.8 5.8-5.8h9.9l-3.1 11.6H7.4c-3.2 0-5.8-2.6-5.8-5.8z"
				style="fill-rule:evenodd;clip-rule:evenodd;fill:#06f"
			></path>
			<path
				d="M24.6 4c.2.2.2.6 0 .8L22.5 7l2.2 2.2c.2.2.2.6 0 .8-.2.2-.6.2-.8 0l-2.2-2.2-2.2 2.2c-.2.2-.6.2-.8 0-.2-.2-.2-.6 0-.8L20.8 7l-2.2-2.2c-.2-.2-.2-.6 0-.8.2-.2.6-.2.8 0l2.2 2.2L23.8 4c.2-.2.6-.2.8 0z"
				style="fill:#fff"
			></path>
			<path
				d="M12.7 4.1c.2.2.3.6.1.8L8.6 9.8c-.1.1-.2.2-.3.2-.2.1-.5.1-.7-.1L5.4 7.7c-.2-.2-.2-.6 0-.8.2-.2.6-.2.8 0L8 8.6l3.8-4.5c.2-.2.6-.2.9 0z"
				style="fill:#06f"
			></path>
		</svg>
	
			<span>Your Privacy Choices</span></a
		>
	
	</div>
				<div class="flex-shrink-0">
		<div class="dropdown has-caret-up">
			<button
				data-test-id="theme-selector-button"
				class="dropdown-trigger button button-clear button-sm inner-focus theme-dropdown-trigger"
				aria-controls="{{ themeMenuId }}"
				aria-expanded="false"
				title="Theme"
				data-bi-name="theme"
			>
				<span class="icon" aria-hidden="true"><span class="docon docon-sun"></span></span>
				<span>Theme</span>
				<span class="icon expanded-indicator" aria-hidden="true">
					<span class="docon docon-chevron-down-light"></span>
				</span>
			</button>
			<div class="dropdown-menu" id="{{ themeMenuId }}" role="menu">
				<ul class="theme-selector padding-xxs" data-test-id="theme-dropdown-menu">
					<li class="theme display-block">
						<button
							class="button button-clear button-sm theme-control button-block justify-content-flex-start text-align-left"
							data-theme-to="light"
						>
							<span class="theme-light margin-right-xxs">
								<span
									class="theme-selector-icon border display-inline-block has-body-background"
									aria-hidden="true"
								>
									<svg class="svg" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 22 14">
										<rect width="22" height="14" class="has-fill-body-background" />
										<rect x="5" y="5" width="12" height="4" class="has-fill-secondary" />
										<rect x="5" y="2" width="2" height="1" class="has-fill-secondary" />
										<rect x="8" y="2" width="2" height="1" class="has-fill-secondary" />
										<rect x="11" y="2" width="3" height="1" class="has-fill-secondary" />
										<rect x="1" y="1" width="2" height="2" class="has-fill-secondary" />
										<rect x="5" y="10" width="7" height="2" rx="0.3" class="has-fill-primary" />
										<rect x="19" y="1" width="2" height="2" rx="1" class="has-fill-secondary" />
									</svg>
								</span>
							</span>
							<span role="menuitem"> Light </span>
						</button>
					</li>
					<li class="theme display-block">
						<button
							class="button button-clear button-sm theme-control button-block justify-content-flex-start text-align-left"
							data-theme-to="dark"
						>
							<span class="theme-dark margin-right-xxs">
								<span
									class="border theme-selector-icon display-inline-block has-body-background"
									aria-hidden="true"
								>
									<svg class="svg" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 22 14">
										<rect width="22" height="14" class="has-fill-body-background" />
										<rect x="5" y="5" width="12" height="4" class="has-fill-secondary" />
										<rect x="5" y="2" width="2" height="1" class="has-fill-secondary" />
										<rect x="8" y="2" width="2" height="1" class="has-fill-secondary" />
										<rect x="11" y="2" width="3" height="1" class="has-fill-secondary" />
										<rect x="1" y="1" width="2" height="2" class="has-fill-secondary" />
										<rect x="5" y="10" width="7" height="2" rx="0.3" class="has-fill-primary" />
										<rect x="19" y="1" width="2" height="2" rx="1" class="has-fill-secondary" />
									</svg>
								</span>
							</span>
							<span role="menuitem"> Dark </span>
						</button>
					</li>
					<li class="theme display-block">
						<button
							class="button button-clear button-sm theme-control button-block justify-content-flex-start text-align-left"
							data-theme-to="high-contrast"
						>
							<span class="theme-high-contrast margin-right-xxs">
								<span
									class="border theme-selector-icon display-inline-block has-body-background"
									aria-hidden="true"
								>
									<svg class="svg" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 22 14">
										<rect width="22" height="14" class="has-fill-body-background" />
										<rect x="5" y="5" width="12" height="4" class="has-fill-secondary" />
										<rect x="5" y="2" width="2" height="1" class="has-fill-secondary" />
										<rect x="8" y="2" width="2" height="1" class="has-fill-secondary" />
										<rect x="11" y="2" width="3" height="1" class="has-fill-secondary" />
										<rect x="1" y="1" width="2" height="2" class="has-fill-secondary" />
										<rect x="5" y="10" width="7" height="2" rx="0.3" class="has-fill-primary" />
										<rect x="19" y="1" width="2" height="2" rx="1" class="has-fill-secondary" />
									</svg>
								</span>
							</span>
							<span role="menuitem"> High contrast </span>
						</button>
					</li>
				</ul>
			</div>
		</div>
	</div>
			</div>
			<ul class="links" data-bi-name="footerlinks">
				<li class="manage-cookies-holder" hidden=""></li>
				<li>
		
		<a
			data-mscc-ic="false"
			href="https://learn.microsoft.com/en-us/principles-for-ai-generated-content"
			data-bi-name="aiDisclaimer"
			class=" external-link-indicator"
			id=""
			title=""
			>AI Disclaimer</a
		>
	
	</li><li>
		
		<a
			data-mscc-ic="false"
			href="https://learn.microsoft.com/en-us/previous-versions/"
			data-bi-name="archivelink"
			class=" external-link-indicator"
			id=""
			title=""
			>Previous Versions</a
		>
	
	</li> <li>
		
		<a
			data-mscc-ic="false"
			href="https://techcommunity.microsoft.com/t5/microsoft-learn-blog/bg-p/MicrosoftLearnBlog"
			data-bi-name="bloglink"
			class=" external-link-indicator"
			id=""
			title=""
			>Blog</a
		>
	
	</li> <li>
		
		<a
			data-mscc-ic="false"
			href="https://learn.microsoft.com/en-us/contribute"
			data-bi-name="contributorGuide"
			class=" external-link-indicator"
			id=""
			title=""
			>Contribute</a
		>
	
	</li><li>
		
		<a
			data-mscc-ic="false"
			href="https://go.microsoft.com/fwlink/?LinkId=521839"
			data-bi-name="privacy"
			class=" external-link-indicator"
			id=""
			title=""
			>Privacy</a
		>
	
	</li><li>
		
		<a
			data-mscc-ic="false"
			href="https://go.microsoft.com/fwlink/?linkid=2259814"
			data-bi-name="consumer-health-privacy"
			class=" external-link-indicator"
			id=""
			title=""
			>Consumer Health Privacy</a
		>
	
	</li><li>
		
		<a
			data-mscc-ic="false"
			href="https://learn.microsoft.com/en-us/legal/termsofuse"
			data-bi-name="termsofuse"
			class=" external-link-indicator"
			id=""
			title=""
			>Terms of Use</a
		>
	
	</li><li>
		
		<a
			data-mscc-ic="false"
			href="https://www.microsoft.com/legal/intellectualproperty/Trademarks/"
			data-bi-name="trademarks"
			class=" external-link-indicator"
			id=""
			title=""
			>Trademarks</a
		>
	
	</li>
				<li>&copy; Microsoft 2026</li>
			</ul>
		</footer>
	</footer>
			</body>
		</html>
