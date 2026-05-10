---
source_title: Cloud SQL for MySQL Flags
source_url: https://cloud.google.com/sql/docs/mysql/flags
engine: mysql
vendor: gcp
topic: connections
seed_id: gcp-cloud-sql-mysql-flags
captured_at_utc: 2026-05-09T12:11:34.7707415+00:00
---










<!doctype html>
<html 
      lang="en"
      dir="ltr">
  <head>
    <meta name="google-signin-client-id" content="721724668570-nbkv1cfusk7kk4eni4pjvepaus73b13t.apps.googleusercontent.com"><meta name="google-signin-scope"
          content="profile email https://www.googleapis.com/auth/developerprofiles https://www.googleapis.com/auth/developerprofiles.award https://www.googleapis.com/auth/devprofiles.full_control.firstparty"><meta property="og:site_name" content="Google Cloud Documentation">
    <meta property="og:type" content="website"><meta name="theme-color" content="#1a73e8"><meta charset="utf-8">
    <meta content="IE=Edge" http-equiv="X-UA-Compatible">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    

    <link rel="manifest" href="/_pwa/clouddocs/manifest.json"
          crossorigin="use-credentials">
    <link rel="preconnect" href="//www.gstatic.com" crossorigin>
    <link rel="preconnect" href="//fonts.googleapis.com" crossorigin>
    <link rel="preconnect" href="//www.google-analytics.com" crossorigin><link rel="stylesheet" href="//fonts.googleapis.com/css?family=Google+Sans:400,500|Roboto:400,400italic,500,500italic,700,700italic|Roboto+Mono:400,500,700&display=swap">
      <link rel="stylesheet"
            href="//fonts.googleapis.com/css2?family=Material+Icons&family=Material+Symbols+Outlined&display=block"><link rel="stylesheet" href="https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs/css/app.css">
      
        <link rel="stylesheet" href="https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs/css/dark-theme.css" disabled>
      <link rel="shortcut icon" href="https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs/images/favicons/onecloud/favicon.ico">
    <link rel="apple-touch-icon" href="https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs/images/favicons/onecloud/super_cloud.png"><link rel="canonical" href="https://docs.cloud.google.com/sql/docs/mysql/flags"><link rel="search" type="application/opensearchdescription+xml"
            title="Google Cloud Documentation" href="https://docs.cloud.google.com/s/opensearch.xml">
      <link rel="alternate" hreflang="en"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags" /><link rel="alternate" hreflang="x-default" href="https://docs.cloud.google.com/sql/docs/mysql/flags" /><link rel="alternate" hreflang="zh-Hans"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=zh-cn" /><link rel="alternate" hreflang="zh-Hant"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=zh-tw" /><link rel="alternate" hreflang="fr"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=fr" /><link rel="alternate" hreflang="de"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=de" /><link rel="alternate" hreflang="he"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=he" /><link rel="alternate" hreflang="id"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=id" /><link rel="alternate" hreflang="it"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=it" /><link rel="alternate" hreflang="ja"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=ja" /><link rel="alternate" hreflang="ko"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=ko" /><link rel="alternate" hreflang="pt-BR"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=pt-br" /><link rel="alternate" hreflang="pt"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=pt" /><link rel="alternate" hreflang="es"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=es" /><link rel="alternate" hreflang="es-419"
          href="https://docs.cloud.google.com/sql/docs/mysql/flags?hl=es-419" /><link rel="alternate" hreflang="en"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags" /><link rel="alternate" hreflang="x-default" href="https://berlin.devsitetest.how/sql/docs/mysql/flags" /><link rel="alternate" hreflang="zh-Hans"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=zh-cn" /><link rel="alternate" hreflang="zh-Hant"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=zh-tw" /><link rel="alternate" hreflang="fr"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=fr" /><link rel="alternate" hreflang="de"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=de" /><link rel="alternate" hreflang="he"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=he" /><link rel="alternate" hreflang="id"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=id" /><link rel="alternate" hreflang="it"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=it" /><link rel="alternate" hreflang="ja"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=ja" /><link rel="alternate" hreflang="ko"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=ko" /><link rel="alternate" hreflang="pt-BR"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=pt-br" /><link rel="alternate" hreflang="pt"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=pt" /><link rel="alternate" hreflang="es"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=es" /><link rel="alternate" hreflang="es-419"
          href="https://berlin.devsitetest.how/sql/docs/mysql/flags?hl=es-419" /><title>Configure database flags &nbsp;|&nbsp; Cloud SQL for MySQL &nbsp;|&nbsp; Google Cloud Documentation</title>

<meta property="og:title" content="Configure database flags &nbsp;|&nbsp; Cloud SQL for MySQL &nbsp;|&nbsp; Google Cloud Documentation"><meta property="og:url" content="https://docs.cloud.google.com/sql/docs/mysql/flags"><meta property="og:image" content="https://docs.cloud.google.com/_static/cloud/images/social-icon-google-cloud-1200-630.png">
  <meta property="og:image:width" content="1200">
  <meta property="og:image:height" content="630"><meta property="og:locale" content="en"><meta name="twitter:card" content="summary_large_image"><script type="application/ld+json">
  {
    "@context": "https://schema.org",
    "@type": "Article",
    
    "headline": "Configure database flags"
  }
</script><script type="application/ld+json">
  {
    "@context": "https://schema.org",
    "@type": "BreadcrumbList",
    "itemListElement": [{
      "@type": "ListItem",
      "position": 1,
      "name": "Cloud SQL",
      "item": "https://docs.cloud.google.com/sql/docs"
    },{
      "@type": "ListItem",
      "position": 2,
      "name": "MySQL",
      "item": "https://docs.cloud.google.com/sql/docs/mysql"
    },{
      "@type": "ListItem",
      "position": 3,
      "name": "Configure database flags",
      "item": "https://docs.cloud.google.com/sql/docs/mysql/flags"
    }]
  }
  </script>
  

  

  


    </head>
  <body class="color-scheme--light"
        template="page"
        theme="clouddocs-theme"
        type="article"
        
        appearance
        
        layout="docs"
        
        
        free-trial
        
        
        display-toc
        pending>
  
    <devsite-progress type="indeterminate" id="app-progress"></devsite-progress>
  
  
    <a href="#main-content" class="skip-link button">
      
      Skip to main content
    </a>
    <section class="devsite-wrapper">
      <devsite-cookie-notification-bar></devsite-cookie-notification-bar>
        <cloudx-track userCountry="TW"></cloudx-track>

<devsite-header role="banner" keep-tabs-visible>
  
    





















<div class="devsite-header--inner" data-nosnippet>
  <div class="devsite-top-logo-row-wrapper-wrapper">
    <div class="devsite-top-logo-row-wrapper">
      <div class="devsite-top-logo-row">
        <button type="button" id="devsite-hamburger-menu"
          class="devsite-header-icon-button button-flat material-icons gc-analytics-event"
          data-category="Site-Wide Custom Events"
          data-label="Navigation menu button"
          visually-hidden
          aria-label="Open menu">
        </button>
        
<div class="devsite-product-name-wrapper">

  <a href="/" class="devsite-site-logo-link gc-analytics-event"
   data-category="Site-Wide Custom Events" data-label="Site logo" track-type="globalNav"
   track-name="googleCloudDocumentation" track-metadata-position="nav"
   track-metadata-eventDetail="nav">
  
  <picture>
    
    <source srcset="https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs/images/lockup-dark-theme.svg"
            media="(prefers-color-scheme: dark)"
            class="devsite-dark-theme">
    
    <img src="https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs/images/lockup.svg" class="devsite-site-logo" alt="Google Cloud Documentation">
  </picture>
  
</a>



</div>
        <div class="devsite-top-logo-row-middle">
          <div class="devsite-header-upper-tabs">
            
              
              
  <devsite-tabs class="upper-tabs">

    <nav class="devsite-tabs-wrapper" aria-label="Upper tabs">
      
        
          <tab class="devsite-dropdown
    
    devsite-active
    devsite-clickable
    ">
  
    <a href="https://docs.cloud.google.com/docs"
    class="devsite-tabs-content gc-analytics-event "
      track-metadata-eventdetail="https://docs.cloud.google.com/docs"
    
       track-type="nav"
       track-metadata-position="nav - docs-home"
       track-metadata-module="primary nav"
       aria-label="Technology areas, selected" 
       
         
           data-category="Site-Wide Custom Events"
         
           data-label="Tab: Technology areas"
         
           track-name="docs-home"
         
           track-link-column-type="single-column"
         
       >
    Technology areas
  
    </a>
    
      <button
         aria-haspopup="menu"
         aria-expanded="false"
         aria-label="Dropdown menu for Technology areas"
         track-type="nav"
         track-metadata-eventdetail="https://docs.cloud.google.com/docs"
         track-metadata-position="nav - docs-home"
         track-metadata-module="primary nav"
         
          
            data-category="Site-Wide Custom Events"
          
            data-label="Tab: Technology areas"
          
            track-name="docs-home"
          
            track-link-column-type="single-column"
          
        
         class="devsite-tabs-dropdown-toggle devsite-icon devsite-icon-arrow-drop-down"></button>
    
  
  <div class="devsite-tabs-dropdown" role="menu" aria-label="submenu" hidden>
    <div class="devsite-tabs-dropdown-content">
      
        <button class="devsite-tabs-close-button material-icons button-flat gc-analytics-event"
                data-category="Site-Wide Custom Events"
                data-label="Close dropdown menu"
                aria-label="Close dropdown menu"
                track-type="nav"
                track-name="close"
                track-metadata-eventdetail="#"
                track-metadata-position="nav - docs-home"
                track-metadata-module="tertiary nav">close</button>
      
      
        <div class="devsite-tabs-dropdown-column
                    ">
          
            <ul class="devsite-tabs-dropdown-section
                       ">
              
              
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/ai-ml"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/ai-ml"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      AI and ML
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/application-development"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/application-development"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Application development
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/application-hosting"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/application-hosting"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Application hosting
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/compute-area"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/compute-area"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Compute
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/data"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/data"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Data analytics and pipelines
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/databases"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/databases"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Databases
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/dhm-cloud"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/dhm-cloud"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Distributed, hybrid, and multicloud
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/industry"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/industry"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Industry solutions
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/migration"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/migration"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Migration
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/networking"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/networking"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Networking
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/observability"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/observability"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Observability and monitoring
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/security"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/security"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Security
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/storage"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/storage"
                     track-metadata-position="nav - docs-home"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Storage
                    </div>
                    
                  </a>
                </li>
              
            </ul>
          
        </div>
      
    </div>
  </div>
</tab>
        
      
        
          <tab class="devsite-dropdown
    
    
    devsite-clickable
    ">
  
    <a href="https://docs.cloud.google.com/docs/cross-product-overviews"
    class="devsite-tabs-content gc-analytics-event "
      track-metadata-eventdetail="https://docs.cloud.google.com/docs/cross-product-overviews"
    
       track-type="nav"
       track-metadata-position="nav - crossproduct"
       track-metadata-module="primary nav"
       
       
         
           data-category="Site-Wide Custom Events"
         
           data-label="Tab: Cross-product tools"
         
           track-name="crossproduct"
         
           track-link-column-type="single-column"
         
       >
    Cross-product tools
  
    </a>
    
      <button
         aria-haspopup="menu"
         aria-expanded="false"
         aria-label="Dropdown menu for Cross-product tools"
         track-type="nav"
         track-metadata-eventdetail="https://docs.cloud.google.com/docs/cross-product-overviews"
         track-metadata-position="nav - crossproduct"
         track-metadata-module="primary nav"
         
          
            data-category="Site-Wide Custom Events"
          
            data-label="Tab: Cross-product tools"
          
            track-name="crossproduct"
          
            track-link-column-type="single-column"
          
        
         class="devsite-tabs-dropdown-toggle devsite-icon devsite-icon-arrow-drop-down"></button>
    
  
  <div class="devsite-tabs-dropdown" role="menu" aria-label="submenu" hidden>
    <div class="devsite-tabs-dropdown-content">
      
        <button class="devsite-tabs-close-button material-icons button-flat gc-analytics-event"
                data-category="Site-Wide Custom Events"
                data-label="Close dropdown menu"
                aria-label="Close dropdown menu"
                track-type="nav"
                track-name="close"
                track-metadata-eventdetail="#"
                track-metadata-position="nav - crossproduct"
                track-metadata-module="tertiary nav">close</button>
      
      
        <div class="devsite-tabs-dropdown-column
                    ">
          
            <ul class="devsite-tabs-dropdown-section
                       ">
              
              
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/access-resources"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/access-resources"
                     track-metadata-position="nav - crossproduct"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Access and resources management
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/costs-usage"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/costs-usage"
                     track-metadata-position="nav - crossproduct"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Costs and usage management
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/iac"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/iac"
                     track-metadata-position="nav - crossproduct"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      Infrastructure as code
                    </div>
                    
                  </a>
                </li>
              
                <li class="devsite-nav-item">
                  <a href="https://docs.cloud.google.com/docs/devtools"
                    
                     track-type="nav"
                     track-metadata-eventdetail="https://docs.cloud.google.com/docs/devtools"
                     track-metadata-position="nav - crossproduct"
                     track-metadata-module="tertiary nav"
                     
                     tooltip
                  >
                    
                    <div class="devsite-nav-item-title">
                      SDK, languages, frameworks, and tools
                    </div>
                    
                  </a>
                </li>
              
            </ul>
          
        </div>
      
    </div>
  </div>
</tab>
        
      
    </nav>

  </devsite-tabs>

            
           </div>
          
<devsite-search
    enable-signin
    enable-search
    enable-suggestions
      
    
    enable-search-summaries
    project-name="Cloud SQL for MySQL"
    tenant-name="Google Cloud Documentation"
    project-scope="/sql/docs/mysql"
    url-scoped="https://docs.cloud.google.com/s/results/sql/docs/mysql"
    
    
    
    >
  <form class="devsite-search-form" action="https://docs.cloud.google.com/s/results" method="GET">
    <div class="devsite-search-container">
      <button type="button"
              search-open
              class="devsite-search-button devsite-header-icon-button button-flat material-icons"
              
              aria-label="Open search"></button>
      <div class="devsite-searchbox">
        <input
          aria-activedescendant=""
          aria-autocomplete="list"
          
          aria-label="Search"
          aria-expanded="false"
          aria-haspopup="listbox"
          autocomplete="off"
          class="devsite-search-field devsite-search-query"
          name="q"
          
          placeholder="Search"
          role="combobox"
          type="text"
          value=""
          >
          <div class="devsite-search-image material-icons" aria-hidden="true">
            
              <svg class="devsite-search-ai-image" width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                  <g clip-path="url(#clip0_6641_386)">
                    <path d="M19.6 21L13.3 14.7C12.8 15.1 12.225 15.4167 11.575 15.65C10.925 15.8833 10.2333 16 9.5 16C7.68333 16 6.14167 15.375 4.875 14.125C3.625 12.8583 3 11.3167 3 9.5C3 7.68333 3.625 6.15 4.875 4.9C6.14167 3.63333 7.68333 3 9.5 3C10.0167 3 10.5167 3.05833 11 3.175C11.4833 3.275 11.9417 3.43333 12.375 3.65L10.825 5.2C10.6083 5.13333 10.3917 5.08333 10.175 5.05C9.95833 5.01667 9.73333 5 9.5 5C8.25 5 7.18333 5.44167 6.3 6.325C5.43333 7.19167 5 8.25 5 9.5C5 10.75 5.43333 11.8167 6.3 12.7C7.18333 13.5667 8.25 14 9.5 14C10.6667 14 11.6667 13.625 12.5 12.875C13.35 12.1083 13.8417 11.15 13.975 10H15.975C15.925 10.6333 15.7833 11.2333 15.55 11.8C15.3333 12.3667 15.05 12.8667 14.7 13.3L21 19.6L19.6 21ZM17.5 12C17.5 10.4667 16.9667 9.16667 15.9 8.1C14.8333 7.03333 13.5333 6.5 12 6.5C13.5333 6.5 14.8333 5.96667 15.9 4.9C16.9667 3.83333 17.5 2.53333 17.5 0.999999C17.5 2.53333 18.0333 3.83333 19.1 4.9C20.1667 5.96667 21.4667 6.5 23 6.5C21.4667 6.5 20.1667 7.03333 19.1 8.1C18.0333 9.16667 17.5 10.4667 17.5 12Z" fill="#5F6368"/>
                  </g>
                <defs>
                <clipPath id="clip0_6641_386">
                <rect width="24" height="24" fill="white"/>
                </clipPath>
                </defs>
              </svg>
            
          </div>
          <div class="devsite-search-shortcut-icon-container" aria-hidden="true">
            <kbd class="devsite-search-shortcut-icon">/</kbd>
          </div>
      </div>
    </div>
  </form>
  <button type="button"
          search-close
          class="devsite-search-button devsite-header-icon-button button-flat material-icons"
          
          aria-label="Close search"></button>
</devsite-search>

        </div>

        

  

  
    <a class="devsite-header-link devsite-top-button button gc-analytics-event button-with-icon"
    href="//console.cloud.google.com/"
    data-category="Site-Wide Custom Events"
    data-label="Site header link: Console"
    
      
        track-metadata-eventDetail="nav"
      
        referrerpolicy="no-referrer-when-downgrade"
      
        track-metadata-position="nav"
      
        track-type="globalNav"
      
        track-name="console"
      
    >
  Console
</a>
  

  

  <devsite-appearance-selector></devsite-appearance-selector>

  
<devsite-language-selector>
  <ul role="presentation">
    
    
    <li role="presentation">
      <a role="menuitem" lang="en"
        >English</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="de"
        >Deutsch</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="es"
        >Español</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="es_419"
        >Español – América Latina</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="fr"
        >Français</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="id"
        >Indonesia</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="it"
        >Italiano</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="pt"
        >Português</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="pt_br"
        >Português – Brasil</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="he"
        >עברית</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="zh_cn"
        >中文 – 简体</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="zh_tw"
        >中文 – 繁體</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="ja"
        >日本語</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="ko"
        >한국어</a>
    </li>
    
  </ul>
</devsite-language-selector>




        
          <devsite-user 
                        
                        
                          enable-profiles
                        
                        
                          fp-auth
                        
                        id="devsite-user">
            
              
              <span class="button devsite-top-button" aria-hidden="true" visually-hidden>Sign in</span>
            
          </devsite-user>
        
        
        
      </div>
    </div>
  </div>



  <div class="devsite-collapsible-section
    ">
    <div class="devsite-header-background">
      
        
          <div class="devsite-product-id-row"
           >
            <div class="devsite-product-description-row">
              
                
                <div class="devsite-product-id">
                  
                    
  
  <a href="https://docs.cloud.google.com/sql/docs/mysql">
    
  <div class="devsite-product-logo-container"
       
       
       
    size="medium"
  >
  
    <picture>
      
      <source class="devsite-dark-theme"
              media="(prefers-color-scheme: dark)"
              srcset=" /_static/clouddocs/images/icons/products/sql-white.svg"
              sizes="64px">
      
      <img class="devsite-product-logo"
           alt=""
           src="https://docs.cloud.google.com/_static/clouddocs/images/icons/products/sql-color.svg"
           srcset=" /_static/clouddocs/images/icons/products/sql-color.svg"
           sizes="64px"
           loading="lazy"
           >
    </picture>
  
  </div>
  
  </a>
  

                  
                  
                  
                    <ul class="devsite-breadcrumb-list"
  
    aria-label="Lower header breadcrumb">
  
  <li class="devsite-breadcrumb-item
             ">
    
    
    
      
        
  <a href="https://docs.cloud.google.com/sql/docs"
      
        class="devsite-breadcrumb-link gc-analytics-event"
      
        data-category="Site-Wide Custom Events"
      
        data-label="Lower Header"
      
        data-value="1"
      
        track-type="globalNav"
      
        track-name="breadcrumb"
      
        track-metadata-position="1"
      
        track-metadata-eventdetail="Cloud SQL"
      
    >
    
          Cloud SQL
        
  </a>
  
      
    
  </li>
  
  <li class="devsite-breadcrumb-item
             ">
    
      
      <div class="devsite-breadcrumb-guillemet material-icons" aria-hidden="true"></div>
    
    
    
      
        
  <a href="https://docs.cloud.google.com/sql/docs/mysql"
      
        class="devsite-breadcrumb-link gc-analytics-event"
      
        data-category="Site-Wide Custom Events"
      
        data-label="Lower Header"
      
        data-value="2"
      
        track-type="globalNav"
      
        track-name="breadcrumb"
      
        track-metadata-position="2"
      
        track-metadata-eventdetail="Cloud SQL for MySQL"
      
    >
    
          MySQL
        
  </a>
  
      
    
  </li>
  
</ul>
                </div>
                
              
              
            </div>
            
              <div class="devsite-product-button-row">
  

  
  <a href="//console.cloud.google.com/freetrial"
  
    class="cloud-free-trial-button button button-primary
      "
    
    
      
        track-metadata-eventDetail="nav"
      
        referrerpolicy="no-referrer-when-downgrade"
      
        track-metadata-position="nav"
      
        track-type="freeTrial"
      
        track-name="gcpCta"
      
    
    >Start free</a>

</div>
            
          </div>
          
        
      
      
        <div class="devsite-doc-set-nav-row">
          
          
            
            
  <devsite-tabs class="lower-tabs">

    <nav class="devsite-tabs-wrapper" aria-label="Lower tabs">
      
        
          <tab  >
            
    <a href="https://docs.cloud.google.com/sql/docs/mysql"
    class="devsite-tabs-content gc-analytics-event "
      track-metadata-eventdetail="https://docs.cloud.google.com/sql/docs/mysql"
    
       track-type="nav"
       track-metadata-position="nav - overview"
       track-metadata-module="primary nav"
       
       
         
           data-category="Site-Wide Custom Events"
         
           data-label="Tab: Overview"
         
           track-name="overview"
         
       >
    Overview
  
    </a>
    
  
          </tab>
        
      
        
          <tab  class="devsite-active">
            
    <a href="https://docs.cloud.google.com/sql/docs/mysql/features"
    class="devsite-tabs-content gc-analytics-event "
      track-metadata-eventdetail="https://docs.cloud.google.com/sql/docs/mysql/features"
    
       track-type="nav"
       track-metadata-position="nav - guides"
       track-metadata-module="primary nav"
       aria-label="Guides, selected" 
       
         
           data-category="Site-Wide Custom Events"
         
           data-label="Tab: Guides"
         
           track-name="guides"
         
       >
    Guides
  
    </a>
    
  
          </tab>
        
      
        
          <tab  >
            
    <a href="https://docs.cloud.google.com/sql/docs/mysql/apis"
    class="devsite-tabs-content gc-analytics-event "
      track-metadata-eventdetail="https://docs.cloud.google.com/sql/docs/mysql/apis"
    
       track-type="nav"
       track-metadata-position="nav - reference"
       track-metadata-module="primary nav"
       
       
         
           data-category="Site-Wide Custom Events"
         
           data-label="Tab: Reference"
         
           track-name="reference"
         
       >
    Reference
  
    </a>
    
  
          </tab>
        
      
        
          <tab  >
            
    <a href="https://docs.cloud.google.com/sql/docs/mysql/samples"
    class="devsite-tabs-content gc-analytics-event "
      track-metadata-eventdetail="https://docs.cloud.google.com/sql/docs/mysql/samples"
    
       track-type="nav"
       track-metadata-position="nav - samples"
       track-metadata-module="primary nav"
       
       
         
           data-category="Site-Wide Custom Events"
         
           data-label="Tab: Samples"
         
           track-name="samples"
         
       >
    Samples
  
    </a>
    
  
          </tab>
        
      
        
          <tab  >
            
    <a href="https://docs.cloud.google.com/sql/docs/mysql/resources"
    class="devsite-tabs-content gc-analytics-event "
      track-metadata-eventdetail="https://docs.cloud.google.com/sql/docs/mysql/resources"
    
       track-type="nav"
       track-metadata-position="nav - resources"
       track-metadata-module="primary nav"
       
       
         
           data-category="Site-Wide Custom Events"
         
           data-label="Tab: Resources"
         
           track-name="resources"
         
       >
    Resources
  
    </a>
    
  
          </tab>
        
      
    </nav>

  </devsite-tabs>

          
          
        </div>
      
    </div>
  </div>

</div>



  

  
</devsite-header>
        <devsite-book-nav scrollbars >
          
            





















<div class="devsite-book-nav-filter"
     >
  <span class="filter-list-icon material-icons" aria-hidden="true"></span>
  <input type="text"
         placeholder="Filter"
         
         aria-label="Type to filter"
         role="searchbox">
  
  <span class="filter-clear-button hidden"
        data-title="Clear filter"
        aria-label="Clear filter"
        role="button"
        tabindex="0"></span>
</div>

<nav class="devsite-book-nav devsite-nav nocontent" data-nosnippet
     aria-label="Side menu">
  <div class="devsite-mobile-header">
    <button type="button"
            id="devsite-close-nav"
            class="devsite-header-icon-button button-flat material-icons gc-analytics-event"
            data-category="Site-Wide Custom Events"
            data-label="Close navigation"
            aria-label="Close navigation">
    </button>
    <div class="devsite-product-name-wrapper">

  <a href="/" class="devsite-site-logo-link gc-analytics-event"
   data-category="Site-Wide Custom Events" data-label="Site logo" track-type="globalNav"
   track-name="googleCloudDocumentation" track-metadata-position="nav"
   track-metadata-eventDetail="nav">
  
  <picture>
    
    <source srcset="https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs/images/lockup-dark-theme.svg"
            media="(prefers-color-scheme: dark)"
            class="devsite-dark-theme">
    
    <img src="https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs/images/lockup.svg" class="devsite-site-logo" alt="Google Cloud Documentation">
  </picture>
  
</a>


</div>
  </div>

  <div class="devsite-book-nav-wrapper">
    <div class="devsite-mobile-nav-top">
      
        <ul class="devsite-nav-list">
          
            <li class="devsite-nav-item">
              
  
  <a href="/docs"
    
       class="devsite-nav-title gc-analytics-event
              
              devsite-nav-active"
    

    
      
        data-category="Site-Wide Custom Events"
      
        data-label="Tab: Technology areas"
      
        track-name="docs-home"
      
        track-link-column-type="single-column"
      
    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Technology areas"
     track-type="globalNav"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Technology areas
   </span>
    
  
  </a>
  

  
    <ul class="devsite-nav-responsive-tabs devsite-nav-has-menu
               ">
      
<li class="devsite-nav-item">

  
  <span
    
       class="devsite-nav-title"
       tooltip
    
    
      
        data-category="Site-Wide Custom Events"
      
        data-label="Tab: Technology areas"
      
        track-name="docs-home"
      
        track-link-column-type="single-column"
      
    >
  
    <span class="devsite-nav-text" tooltip menu="Technology areas">
      More
   </span>
    
    <span class="devsite-nav-icon material-icons" data-icon="forward"
          menu="Technology areas">
    </span>
    
  
  </span>
  

</li>

    </ul>
  
              
                <ul class="devsite-nav-responsive-tabs">
                  
                    
                    
                    
                    <li class="devsite-nav-item">
                      
  
  <a href="/sql/docs/mysql"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
      
        data-category="Site-Wide Custom Events"
      
        data-label="Tab: Overview"
      
        track-name="overview"
      
    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Overview"
     track-type="globalNav"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Overview
   </span>
    
  
  </a>
  

  
                    </li>
                  
                    
                    
                    
                    <li class="devsite-nav-item">
                      
  
  <a href="/sql/docs/mysql/features"
    
       class="devsite-nav-title gc-analytics-event
              
              devsite-nav-active"
    

    
      
        data-category="Site-Wide Custom Events"
      
        data-label="Tab: Guides"
      
        track-name="guides"
      
    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Guides"
     track-type="globalNav"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip menu="_book">
      Guides
   </span>
    
  
  </a>
  

  
                    </li>
                  
                    
                    
                    
                    <li class="devsite-nav-item">
                      
  
  <a href="/sql/docs/mysql/apis"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
      
        data-category="Site-Wide Custom Events"
      
        data-label="Tab: Reference"
      
        track-name="reference"
      
    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Reference"
     track-type="globalNav"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Reference
   </span>
    
  
  </a>
  

  
                    </li>
                  
                    
                    
                    
                    <li class="devsite-nav-item">
                      
  
  <a href="/sql/docs/mysql/samples"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
      
        data-category="Site-Wide Custom Events"
      
        data-label="Tab: Samples"
      
        track-name="samples"
      
    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Samples"
     track-type="globalNav"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Samples
   </span>
    
  
  </a>
  

  
                    </li>
                  
                    
                    
                    
                    <li class="devsite-nav-item">
                      
  
  <a href="/sql/docs/mysql/resources"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
      
        data-category="Site-Wide Custom Events"
      
        data-label="Tab: Resources"
      
        track-name="resources"
      
    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Resources"
     track-type="globalNav"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Resources
   </span>
    
  
  </a>
  

  
                    </li>
                  
                </ul>
              
            </li>
          
            <li class="devsite-nav-item">
              
  
  <a href="/docs/cross-product-overviews"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
      
        data-category="Site-Wide Custom Events"
      
        data-label="Tab: Cross-product tools"
      
        track-name="crossproduct"
      
        track-link-column-type="single-column"
      
    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Cross-product tools"
     track-type="globalNav"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Cross-product tools
   </span>
    
  
  </a>
  

  
    <ul class="devsite-nav-responsive-tabs devsite-nav-has-menu
               ">
      
<li class="devsite-nav-item">

  
  <span
    
       class="devsite-nav-title"
       tooltip
    
    
      
        data-category="Site-Wide Custom Events"
      
        data-label="Tab: Cross-product tools"
      
        track-name="crossproduct"
      
        track-link-column-type="single-column"
      
    >
  
    <span class="devsite-nav-text" tooltip menu="Cross-product tools">
      More
   </span>
    
    <span class="devsite-nav-icon material-icons" data-icon="forward"
          menu="Cross-product tools">
    </span>
    
  
  </span>
  

</li>

    </ul>
  
              
            </li>
          
          
    
    
<li class="devsite-nav-item">

  
  <a href="//console.cloud.google.com/"
    
       class="devsite-nav-title gc-analytics-event button-with-icon"
    

    
      
        track-metadata-eventDetail="nav"
      
        referrerpolicy="no-referrer-when-downgrade"
      
        track-metadata-position="nav"
      
        track-type="globalNav"
      
        track-name="console"
      
    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Console"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Console
   </span>
    
  
  </a>
  

</li>

  
          
        </ul>
      
    </div>
    
      <div class="devsite-mobile-nav-bottom">
        
          
          <ul class="devsite-nav-list" menu="_book">
            <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Discover</span>
      </div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/introduction"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Product overview</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/editions-intro"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Cloud SQL editions overview</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/features"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Cloud SQL for MySQL features</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/key-terms"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Key terms</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Get started</span>
      </div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Free trial instances</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/free-trial-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Free trial instance overview</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/create-free-trial-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Create a free trial instance</span></a></li></ul></div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/create-query-database"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Quickstart: Create and query a database in the Cloud Console</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/create-network-instance-import-data"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Create an instance in a private network and then import a database</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Connect from a Cloud Service</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-instance-cloud-shell"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Quickstart: Connect from Cloud Shell</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-instance-cloud-run"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Quickstart: Connect from Cloud Run</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-instance-kubernetes"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Quickstart: Connect from Google Kubernetes Engine</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-instance-app-engine"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Quickstart: Connect from App Engine standard environment</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-instance-app-engine-flexible"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Quickstart: Connect from App Engine flexible environment</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-instance-compute-engine"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Quickstart: Connect from Compute Engine</span></a></li></ul></div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-instance-private-ip"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Quickstart: Connect using private IP</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-instance-auth-proxy"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Quickstart: Connect using the Cloud SQL Auth proxy</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-instance-local-computer"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Quickstart: Connect from your local computer</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Plan and prepare</span>
      </div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/plan-prepare-overview"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Overview</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/choose-edition"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Choose a Cloud SQL edition</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/machine-series-overview"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Choose a machine series</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/storage-options-overview"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Choose a storage option</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/region-availability-overview"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Region availability</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/data-cache"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Data cache overview</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Create and manage</span>
      </div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Instances</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/create-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Create instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/edit-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Edit instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/clone-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Clone instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/start-stop-restart-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Start, stop, and restart instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/label-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Label instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/delete-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Delete instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/deletion-protection"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Prevent deletion of an instance</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/instance-settings"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Supported instance settings</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/instance-info"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>View instance information</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/flags"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure database flags</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/locations"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage instance locations</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/manage-connectivity-tests"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage connectivity tests</span></a></li><li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Manage capacity</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/about-storage-shrink"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About storage shrink</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/shrink-instance-storage-capacity"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Shrink instance storage capacity</span></a></li></ul></div></li><li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Manage maintenance updates</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/maintenance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Maintenance updates on instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/set-maintenance-window"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>View and set maintenance windows</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/self-service-maintenance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Perform self-service maintenance</span></a></li></ul></div></li><li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Upgrade</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Upgrade an instance to Cloud SQL Enterprise Plus edition</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/upgrade-cloud-sql-instance-to-enterprise-plus-in-place"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Upgrade an instance by using in-place upgrade</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/upgrade-cloud-sql-instance-to-enterprise-plus-ip-allowlists"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Upgrade an instance by using IP allowlists</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/upgrade-cloud-sql-instance-to-enterprise-plus-vpc-peering"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Upgrade an instance by using VPC peering</span></a></li></ul></div></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/upgrade-cloud-sql-instance-new-network-architecture"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Upgrade an instance to the new network architecture</span></a></li><li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Upgrade the database major version</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/upgrade-major-db-version-inplace"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Upgrade the database major version in-place</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/troubleshooting-in-place-major-version-upgrade-to-8.0"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Troubleshooting in-place major version upgrade to MySQL 8.0</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/known-issues-in-mysql-8.0-minor-versions"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Known issues in MySQL 8.0 minor versions</span></a></li></ul></div></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/upgrade-major-db-version-migrate"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Upgrade the database major version by migrating data</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/upgrade-minor-db-version"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Upgrade the database minor version</span></a></li></ul></div></li><li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Use best practices</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/best-practices"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>General best practices</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/operational-guidelines"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Operational guidelines</span></a></li></ul></div></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Databases</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/create-manage-databases"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Create and manage databases</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/charset-collation"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Update the character set and collation for a database</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/executesql-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Execute SQL statements using the Cloud SQL Data API</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Users</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/users"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About MySQL users</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/built-in-authentication"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Cloud SQL built-in database authentication</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/create-manage-users"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage users with built-in authentication</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable
           devsite-nav-preview"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Cloud SQL Studio</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/manage-data-using-studio"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage your data using Cloud SQL Studio</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/write-sql-gemini"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Write SQL with Gemini Assistance</span></a></li></ul></div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/dataplex-catalog-integration"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage your resources using Knowledge Catalog</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Secure and control access</span>
      </div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Overview</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/instance-access-control"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About access control</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/data-residency-overview"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Data residency overview</span></a></li></ul></div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/use-secret-manager"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use Secret Manager to handle secrets in Cloud SQL</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Organization policies</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/org-policy/org-policy"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Cloud SQL organization policies</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/org-policy/configure-org-policy"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Add predefined organization policies</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/org-policy/custom-org-policy"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Add custom organization policies</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Identity and Access Management (IAM)</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/iam-authentication"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>IAM authentication</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/roles-and-permissions"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Roles and permissions</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/iam-conditions"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use IAM Conditions</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/create-edit-iam-instances"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure instances for IAM database authentication</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/add-manage-iam-users"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage users with IAM database authentication</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/iam-logins"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Log in using IAM database authentication</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Fine-grained access control with tags</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/tags"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Access control with Google Cloud tags</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/manage-tags"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Attach and manage tags on Cloud SQL instances</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Use encryption</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/client-side-encryption"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About client-side encryption</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/cmek"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About customer-managed encryption keys (CMEK)</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/configure-cmek"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use customer-managed encryption keys (CMEK)</span></a></li></ul></div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/admin-api/configure-service-controls"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure VPC Service Controls</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/use-brute-force-protection"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use Cloud SQL brute-force protection</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Connect</span>
      </div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/connection-options"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Choose how to connect to Cloud SQL</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/authorize-networks"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Authorize with authorized networks</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Connect to an instance using public IP</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/configure-ip"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure public IP</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Connect to an instance using private IP</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/private-ip"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Learn about using private IP</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/configure-private-ip"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure private IP</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/configure-private-services-access"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure private services access</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-to-instance-using-write-endpoint"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect to an instance using a write endpoint</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/about-private-service-connect"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Private Service Connect overview</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/configure-private-service-connect"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect to an instance using Private Service Connect</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/configure-private-services-access-and-private-service-connect"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure both private services access and Private Service Connect</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-multiple-vpcs"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect to your instance across Multiple VPCs</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Connect using SSL/TLS certificates</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/authorize-ssl"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Authorize with SSL/TLS certificates</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/configure-ssl-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure SSL/TLS certificates</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/manage-ssl-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage SSL/TLS certificates</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/customer-managed-ca"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use a customer-managed certificate authority (CA)</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/custom-dns-name"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Set up a custom DNS name</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Connect using Cloud SQL Language Connectors</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/language-connectors"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Cloud SQL Language Connectors overview</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-connectors"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect using the Cloud SQL Language Connectors</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Connect using the Cloud SQL Auth Proxy</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/sql-proxy"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About the Cloud SQL Auth Proxy</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-auth-proxy"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect using the Cloud SQL Auth Proxy</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-proxy-operator"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect using Cloud SQL Proxy Operator</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Use Managed Connection Pooling</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/managed-connection-pooling"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Managed Connection Pooling overview</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/configure-mcp"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure Managed Connection Pooling</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Connect from applications</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-admin-ip"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect using a MySQL client</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-run"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect from Cloud Run</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-functions"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect from Cloud Functions</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-app-engine-standard"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect from App Engine (Standard)</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/phpmyadmin-on-app-engine"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use phpMyAdmin on App Engine</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/phpmyadmin-on-cloud-run"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use phpMyAdmin on Cloud Run</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-app-engine-flexible"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect from App Engine (Flexible)</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-compute-engine"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect from Compute Engine</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-kubernetes-engine"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect from Kubernetes Engine</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-build"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect from Cloud Build</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/manage-connections"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage database connections</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/admin-tools"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect from other MySQL tools</span></a></li></ul></div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/connect-to-instance-from-outside-vpc"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Connect to an instance from outside its VPC</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Replicate</span>
      </div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/replication"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About replication in Cloud SQL</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Create and manage replicas</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/replication/create-replica"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Create read replicas</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/replication/manage-replicas"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage read replicas</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/replication/read-replica-indexes"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Create and manage indexes on read replicas</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/replication/cross-region-replicas"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Promote replicas for regional migration or disaster recovery</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/replication/replication-lag"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Replication lag</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Create and manage read pools</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/about-read-pools"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About read pools</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/create-read-pool"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Create a read pool</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/read-pool-autoscaling"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Read pool autoscaling</span></a></li></ul></div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/replication/configure-external-replica"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure external replicas</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Replicate from an external server</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/replication/external-server"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About replicating from an external server</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/replication/configure-replication-from-external"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure Cloud SQL and the external server for replication</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/replication/managed-import-replication-from-external"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use a managed import to set up replication from external databases</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/replication/dump-file-replication-from-external"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use a dump file to set up replication from external databases</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/replication/custom-import-replication-from-external"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use a custom import to set up replication from large external databases</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Migrate data</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/migrate-data-to-cloud-sql-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About data migration in Cloud SQL</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/migrate-xtrabackup-physical-file"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Migrate from a Percona XtraBackup physical file</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/migrate-data"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Migrate from Cloud SQL to an external server</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Availability and disaster recovery (DR)</span>
      </div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/availability"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Availability in Cloud SQL</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/high-availability"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About high availability (HA)</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/configure-ha"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Enable and disable high availability (HA)</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/configure-legacy-ha"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Legacy configuration for high availability (HA)</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/intro-to-cloud-sql-disaster-recovery"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About disaster recovery (DR)</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/use-advanced-disaster-recovery"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use advanced disaster recovery (DR)</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Back up and restore</span>
      </div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Back up an instance</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/backup-recovery/backups"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Cloud SQL backups overview</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/backup-recovery/backup-options"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Choose your backup option</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/backup-recovery/manage-standard-backups"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage standard backups</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/backup-recovery/manage-enhanced-backups"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage enhanced backups</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/backup-recovery/manage-backups-deleted-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage backups for deleted instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/backup-recovery/view-audit-logs-for-automated-backups"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>View audit logs for automated backups</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Restore an instance</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/backup-recovery/restore"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Overview</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/backup-recovery/restoring"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Restore an instance using a backup</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/backup-recovery/configure-pitr"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure point-in-time recovery</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/backup-recovery/pitr"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Perform point-in-time recovery</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Import and export</span>
      </div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/import-export"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Best practices for importing and exporting data</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/import-export/import-export-sql"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Export and import using SQL dump files</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/import-export/import-export-csv"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Export and import using CSV files</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/import-export/import-export-parallel"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Export and import files in parallel</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/import-export/cancel-import-export"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Cancel the import and export of data</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/import-export/checking-status-import-export"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Check the status of import and export operations</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Develop</span>
      </div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Build generative AI applications using Cloud SQL</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/integrate-cloud-sql-with-vertex-ai"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Integrate Cloud SQL with Vertex AI</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/invoke-online-predictions"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Invoke online predictions</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/understand-example-embedding-workflow"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Understand an example of an embedding workflow</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/langchain"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Build LLM-powered applications using LangChain</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Interact with custom models using model endpoint management</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/model-endpoint-overview"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Overview</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/model-endpoint-register-model"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Register a model</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/model-endpoint-embeddings"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Generate embeddings</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/model-endpoint-predictions"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Invoke predictions</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/model-endpoint-management-reference"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Model endpoint management reference</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Vector search</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/vector-search"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Vector search</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/enable-vector-search"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Enable and disable vector embeddings</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/generate-manage-vector-embeddings"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Generate and manage vector embeddings</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/create-manage-vector-indexes"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Create and manage vector indexes</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/search-filter-vector-embeddings"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Search and filter with vector embeddings</span></a></li><li class="devsite-nav-item
           devsite-nav-preview"><a href="/sql/docs/mysql/work-with-vectors-preview"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Work with vector embeddings (Preview)</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-preview"><a href="/sql/docs/mysql/pre-built-tools-with-mcp-toolbox"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use Cloud SQL for MySQL with agents</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/use-cloudsql-mcp"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use the Cloud SQL remote MCP server</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/secure-agent-interactions-mcp"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Best practices for securing agent interactions with MCP</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Use saved queries</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item
           devsite-nav-preview"><a href="/sql/docs/mysql/saved-queries"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Overview</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span></a></li><li class="devsite-nav-item
           devsite-nav-preview"><a href="/sql/docs/mysql/create-manage-saved-queries"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Create and manage saved queries</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-preview"><a href="/sql/docs/mysql/build-data-agents-conversational-analytics"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Build data agents with conversational analytics</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span></a></li>

  <li class="devsite-nav-item
           devsite-nav-expandable
           devsite-nav-preview"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Query database in natural language with QueryData</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item
           devsite-nav-expandable
           devsite-nav-preview"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Use context sets</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/context-sets-overview"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Context sets overview</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/build-context-gemini-cli"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Build context sets using Gemini CLI</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/manage-data-agents"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage context sets in Cloud SQL Studio</span></a></li></ul></div></li><li class="devsite-nav-item
           devsite-nav-expandable
           devsite-nav-preview"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Query your agentic application data</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/data-agent-overview"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>QueryData overview</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/inspect-data-agent"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Test QueryData in Cloud SQL Studio</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/integrate-applications-data-agent"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Integrate QueryData with an application</span></a></li></ul></div></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Monitor and optimize</span>
      </div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/observability"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>About database observability</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-preview"><a href="/sql/docs/mysql/monitor-troubleshoot-with-ai"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Monitor and troubleshoot with AI assistance</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span></a></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Audit</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/audit-logging"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Audit logs</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/db-audit"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>MySQL database auditing</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/use-db-audit"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use MySQL database auditing</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable
           devsite-nav-preview"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Performance capture</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item
           devsite-nav-preview"><a href="/sql/docs/mysql/performance-capture"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Overview</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span></a></li><li class="devsite-nav-item
           devsite-nav-preview"><a href="/sql/docs/mysql/configure-performance-capture"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Configure performance capture</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span></a></li><li class="devsite-nav-item
           devsite-nav-preview"><a href="/sql/docs/mysql/view-performance-capture-logs"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>View performance capture logs</span><span class="devsite-nav-icon material-icons"
        data-icon="preview"
        data-title="Preview"
        aria-hidden="true"></span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Query performance</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/using-query-insights"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use query insights</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/use-index-advisor"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use index advisor</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/monitor-active-queries"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Monitor active queries</span></a></li></ul></div></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>System performance</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/monitor-instance"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Monitor instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/logging"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>View instance logs</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/use-system-insights"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use system insights</span></a></li></ul></div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/use-database-insights-mcp"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Monitor Cloud SQL using the Database Insights MCP server</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-expandable"><div class="devsite-expandable-nav">
      <a class="devsite-nav-toggle" aria-hidden="true"></a><div class="devsite-nav-title devsite-nav-title-no-path" tabindex="0" role="button">
        <span class="devsite-nav-text" tooltip>Apply recommendations</span>
      </div><ul class="devsite-nav-section"><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-create-indexes-join-settings"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Create indexes or reconfigure join settings</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-disable-public-ip"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Disable public IP</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-enable-db-audit"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Enable database auditing</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-require-ssl"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Enforce SSL/TLS encryption</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-enable-ha"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Improve instance reliability by enabling high availability</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-enterprise-plus"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Improve performance with Enterprise Plus</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-high-number-of-open-tables"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Increase the table open cache</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-manage-high-open-tables"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage open tables and open table definitions</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-high-number-of-tables"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Manage high number of tables</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/using-ood-recommender"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Monitor disk availability</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/optimize-cpu-usage"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Optimize high CPU usage</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/optimize-high-memory-usage"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Optimize high memory usage</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-optimize-instances-oom"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Optimize instances with high number of out-of-memory events</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-enable-automated-backups"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Prevent data loss by enabling automated backups</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-increase-backup-retention"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Prevent data loss by increasing backup retention</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-reconfigure-connection-settings"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Reconfigure connection settings</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-reconfigure-logs"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Reconfigure log settings</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-high-tmp-table"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Reconfigure temporary table settings</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-sql-idle"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Reduce idle Cloud SQL instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-sql-overprovisioned"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Reduce overprovisioned Cloud SQL instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-underprovisioned"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Reduce underprovisioned Cloud SQL instances</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-remove-authorized-networks"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Remove authorized networks</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-broad-address"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Remove broad public IP ranges</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-rotate-cert"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Rotate server certificates</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-set-password-policy"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Set instance password policies</span></a></li><li class="devsite-nav-item"><a href="/sql/docs/mysql/recommender-set-user-password-policy"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Set user password policies</span></a></li></ul></div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/looker"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Use Looker with Cloud SQL</span></a></li>

  <li class="devsite-nav-item
           devsite-nav-heading"><div class="devsite-nav-title devsite-nav-title-no-path">
        <span class="devsite-nav-text" tooltip>Troubleshoot</span>
      </div></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/known-issues"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Known issues</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/troubleshooting"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Troubleshoot</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/error-messages"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Error messages</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/debugging-connectivity"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Debug connection issues</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/diagnose-issues"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Diagnose issues</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/orphan-tables"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Orphan tables</span></a></li>

  <li class="devsite-nav-item"><a href="/sql/docs/mysql/issues-updating-storage-capacity"
        class="devsite-nav-title"
      ><span class="devsite-nav-text" tooltip>Issues updating storage capacity</span></a></li>
          </ul>
        
        
          
    
      
      <ul class="devsite-nav-list" menu="Technology areas"
          aria-label="Side menu" hidden>
        
          
            
            
              
<li class="devsite-nav-item">

  
  <a href="/docs/ai-ml"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: AI and ML"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      AI and ML
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/application-development"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Application development"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Application development
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/application-hosting"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Application hosting"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Application hosting
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/compute-area"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Compute"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Compute
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/data"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Data analytics and pipelines"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Data analytics and pipelines
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/databases"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Databases"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Databases
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/dhm-cloud"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Distributed, hybrid, and multicloud"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Distributed, hybrid, and multicloud
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/industry"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Industry solutions"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Industry solutions
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/migration"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Migration"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Migration
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/networking"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Networking"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Networking
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/observability"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Observability and monitoring"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Observability and monitoring
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/security"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Security"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Security
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/storage"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Storage"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Storage
   </span>
    
  
  </a>
  

</li>

            
          
        
      </ul>
    
  
    
      
      <ul class="devsite-nav-list" menu="Cross-product tools"
          aria-label="Side menu" hidden>
        
          
            
            
              
<li class="devsite-nav-item">

  
  <a href="/docs/access-resources"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Access and resources management"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Access and resources management
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/costs-usage"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Costs and usage management"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Costs and usage management
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/iac"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: Infrastructure as code"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      Infrastructure as code
   </span>
    
  
  </a>
  

</li>

            
              
<li class="devsite-nav-item">

  
  <a href="/docs/devtools"
    
       class="devsite-nav-title gc-analytics-event
              
              "
    

    
     data-category="Site-Wide Custom Events"
     data-label="Responsive Tab: SDK, languages, frameworks, and tools"
     track-type="navMenu"
     track-metadata-eventDetail="globalMenu"
     track-metadata-position="nav">
  
    <span class="devsite-nav-text" tooltip >
      SDK, languages, frameworks, and tools
   </span>
    
  
  </a>
  

</li>

            
          
        
      </ul>
    
  
        
        
          
    
  
    
  
    
  
    
  
    
  
        
      </div>
    
  </div>
</nav>
          
        </devsite-book-nav>
      
      <section id="gc-wrapper">
        <main role="main" id="main-content" class="devsite-main-content"
            
              has-book-nav
              has-sidebar
            >
          <div class="devsite-sidebar">
            <div class="devsite-sidebar-content">
                
                <devsite-toc class="devsite-nav"
                            role="navigation"
                            aria-label="On this page"
                            depth="2"
                            scrollbars
                            data-nosnippet
                  ></devsite-toc>
                <devsite-recommendations-sidebar class="nocontent devsite-nav" data-nosnippet>
                </devsite-recommendations-sidebar>
            </div>
          </div>
          <devsite-content>
            
              










<article class="devsite-article">
  
  
  
  
  

  <div class="devsite-article-meta nocontent" role="navigation" data-nosnippet>
    
    
    <ul class="devsite-breadcrumb-list"
  
    aria-label="Breadcrumb">
  
  <li class="devsite-breadcrumb-item
             ">
    
    
    
      
        
  <a href="https://docs.cloud.google.com/"
      
        class="devsite-breadcrumb-link gc-analytics-event"
      
        data-category="Site-Wide Custom Events"
      
        data-label="Breadcrumbs"
      
        data-value="1"
      
        track-type="globalNav"
      
        track-name="breadcrumb"
      
        track-metadata-position="1"
      
        track-metadata-eventdetail="Google Cloud Documentation"
      
    >
    
          Home
        
  </a>
  
      
    
  </li>
  
  <li class="devsite-breadcrumb-item
             ">
    
      
      <div class="devsite-breadcrumb-guillemet material-icons" aria-hidden="true"></div>
    
    
    
      
        
  <a href="https://docs.cloud.google.com/docs"
      
        class="devsite-breadcrumb-link gc-analytics-event"
      
        data-category="Site-Wide Custom Events"
      
        data-label="Breadcrumbs"
      
        data-value="2"
      
        track-type="globalNav"
      
        track-name="breadcrumb"
      
        track-metadata-position="2"
      
        track-metadata-eventdetail="Documentation"
      
    >
    
          Documentation
        
  </a>
  
      
    
  </li>
  
  <li class="devsite-breadcrumb-item
             ">
    
      
      <div class="devsite-breadcrumb-guillemet material-icons" aria-hidden="true"></div>
    
    
    
      
        
  <a href="https://docs.cloud.google.com/docs/databases"
      
        class="devsite-breadcrumb-link gc-analytics-event"
      
        data-category="Site-Wide Custom Events"
      
        data-label="Breadcrumbs"
      
        data-value="3"
      
        track-type="globalNav"
      
        track-name="breadcrumb"
      
        track-metadata-position="3"
      
        track-metadata-eventdetail="Databases"
      
    >
    
          Databases
        
  </a>
  
      
    
  </li>
  
  <li class="devsite-breadcrumb-item
             ">
    
      
      <div class="devsite-breadcrumb-guillemet material-icons" aria-hidden="true"></div>
    
    
    
      
        
  <a href="https://docs.cloud.google.com/sql/docs"
      
        class="devsite-breadcrumb-link gc-analytics-event"
      
        data-category="Site-Wide Custom Events"
      
        data-label="Breadcrumbs"
      
        data-value="4"
      
        track-type="globalNav"
      
        track-name="breadcrumb"
      
        track-metadata-position="4"
      
        track-metadata-eventdetail="Cloud SQL"
      
    >
    
          Cloud SQL
        
  </a>
  
      
    
  </li>
  
  <li class="devsite-breadcrumb-item
             ">
    
      
      <div class="devsite-breadcrumb-guillemet material-icons" aria-hidden="true"></div>
    
    
    
      
        
  <a href="https://docs.cloud.google.com/sql/docs/mysql"
      
        class="devsite-breadcrumb-link gc-analytics-event"
      
        data-category="Site-Wide Custom Events"
      
        data-label="Breadcrumbs"
      
        data-value="5"
      
        track-type="globalNav"
      
        track-name="breadcrumb"
      
        track-metadata-position="5"
      
        track-metadata-eventdetail="Cloud SQL for MySQL"
      
    >
    
          MySQL
        
  </a>
  
      
    
  </li>
  
  <li class="devsite-breadcrumb-item
             ">
    
      
      <div class="devsite-breadcrumb-guillemet material-icons" aria-hidden="true"></div>
    
    
    
      
        
  <a href="https://docs.cloud.google.com/sql/docs/mysql/features"
      
        class="devsite-breadcrumb-link gc-analytics-event"
      
        data-category="Site-Wide Custom Events"
      
        data-label="Breadcrumbs"
      
        data-value="6"
      
        track-type="globalNav"
      
        track-name="breadcrumb"
      
        track-metadata-position="6"
      
        track-metadata-eventdetail=""
      
    >
    
          Guides
        
  </a>
  
      
    
  </li>
  
</ul>
    
      
    <devsite-thumb-rating position="header">
    </devsite-thumb-rating>
  
    
  </div>
  
    <devsite-feedback
  position="header"
  project-name="Cloud SQL for MySQL"
  product-id="82040"
  bucket="documentation"
  context=""
  version="t-devsite-webserver-20260428-r00-rc01.477318040238770238"
  data-label="Send Feedback Button"
  track-type="feedback"
  track-name="sendFeedbackLink"
  track-metadata-position="header"
  class="nocontent"
  data-nosnippet
  
  
    project-feedback-url="https://issuetracker.google.com/issues/new?component=187202"
  
  
    
      project-icon="https://docs.cloud.google.com/_static/clouddocs/images/icons/products/sql-color.svg"
    
  
  
    project-support-url="https://cloud.google.com/sql/docs/mysql/getting-support"
  
  
  >

  <button>
  
    
    Send feedback
  
  </button>
</devsite-feedback>
  
    <h1 class="devsite-page-title" tabindex="-1">
      Configure database flags<devsite-actions hidden data-nosnippet><devsite-feature-tooltip
      ack-key="AckCollectionsBookmarkTooltipDismiss"
      analytics-category="Site-Wide Custom Events"
      analytics-action-show="Callout Profile displayed"
      analytics-action-close="Callout Profile dismissed"
      analytics-label="Create Collection Callout"
      class="devsite-page-bookmark-tooltip nocontent"
      data-nosnippet
      dismiss-button="true"
      id="devsite-collections-dropdown"
      
      dismiss-button-text="Dismiss"

      
      close-button-text="Got it">

    
    
      <devsite-bookmark></devsite-bookmark>
    

    <span slot="popout-heading">
      
      Stay organized with collections
    </span>
    <span slot="popout-contents">
      
      Save and categorize content based on your preferences.
    </span>
  </devsite-feature-tooltip>
    <devsite-llm-tools></devsite-llm-tools></devsite-actions>
  
      
    </h1>
  

  <devsite-toc class="devsite-nav"
    depth="2"
    devsite-toc-embedded
    >
  </devsite-toc>
  
    
  <div class="devsite-article-body clearfix
  ">

  
    
    
    


































































































  



























  







































































































































































































































































































































































































































































































































































































<div style="font-size:medium; margin-bottom:40px; border-top:1px solid black;"
     class="nocontent">
<div style="float:right"></p>


<span style="font-weight:bold">MySQL</span>
<span style="color:light-gray">&nbsp; | &nbsp;</span><a href="/sql/docs/postgres/flags" title="View this page for the PostgreSQL database engine"  track-type="sqlSwitcher" track-name="postgresLink">PostgreSQL</a>
<span style="color:light-gray">&nbsp; | &nbsp;</span><a href="/sql/docs/sqlserver/flags" title="View this page for the SQL Server database engine"   track-type="sqlSwitcher" track-name="sqlServerLink">SQL Server</a>






<p></div>
</div>



<p></p>

<p>This page describes how to configure database flags for Cloud SQL, and
lists the flags that you can set for your instance. You use database flags
for many operations, including adjusting MySQL parameters, adjusting
options, and configuring and tuning an instance.</p>


<aside class="note"><strong>Note:</strong><span> Some database flag settings can affect instance availability or
stability, and remove the instance from the <a href="https://cloud.google.com/sql/sla">Cloud SQL SLA</a>. For
information about these flags, see <a href="/sql/docs/operational-guidelines#database_flag_values">Operational Guidelines</a>.</span></aside>
<p>In some cases, setting
one flag may require that you set another flag to fully enable the
functionality you want to use. For example, to enable
<a href="https://dev.mysql.com/doc/refman/8.0/en/slow-query-log.html" target="_blank">slow query logging</a>,
you must set both the <code translate="no" dir="ltr">slow_query_log</code> flag to <code translate="no" dir="ltr">on</code> and the <code translate="no" dir="ltr">log_output</code> flag
to <code translate="no" dir="ltr">FILE</code> to make your logs available using the Google Cloud console Logs Explorer.</p>



<p>When you set, remove, or modify a flag for a database instance, the database
might be restarted. The flag value is then persisted for the instance until you
remove it. If the instance is the source of a replica, and the instance is
restarted, the replica is also restarted to align with the current configuration
of the instance.</p>

<h2 id="config" data-text="Configure database flags" tabindex="-1">Configure database flags</h2>

<p>The following sections cover common flag management tasks.</p>

<h3 id="set_a_database_flag" data-text="Set a database flag" tabindex="-1">Set a database flag</h3>

<div id="set-flag" class="ds-selector-tabs" data-ds-scope="create-instance">
<section>
  <h3 id="console" data-text="Console" tabindex="-1">Console</h3>
  <ol>
    <li>In the <a href="https://console.cloud.google.com/project/_/sql/instances">Google Cloud console</a>,
select the project that contains the Cloud SQL instance for which you want to set a database flag.</li>
    <li>Open the instance and click <b>Edit</b>.</li>
    <li>Go to the <b>Flags</b> section.</li>
    <li>To set a flag that has not been set on the instance before, click
    <b>Add item</b>, choose the flag from the drop-down menu, and set its value.
    </li>
    <li>Click <b>Save</b> to save your changes.</li>
    <li>Confirm your changes under <b>Flags</b> on the Overview page.</li>
  </ol>
</section>

<section>

<h3 id="gcloud" data-text="gcloud" tabindex="-1">gcloud</h3>

<p>Edit the instance:</p>
<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="Bash">gcloud<span class="devsite-syntax-w"> </span>sql<span class="devsite-syntax-w"> </span>instances<span class="devsite-syntax-w"> </span>patch<span class="devsite-syntax-w"> </span><var translate="no">INSTANCE_NAME</var><span class="devsite-syntax-w"> </span>--database-flags<span class="devsite-syntax-o">=</span><var translate="no"><span class="devsite-syntax-nv">FLAG1</span></var><span class="devsite-syntax-o">=</span><var translate="no">VALUE1</var>,<var translate="no">FLAG2</var><span class="devsite-syntax-o">=</span><var translate="no">VALUE2</var></pre></devsite-code>

<p>This command will overwrite all database flags
previously set. To keep those and add new ones, include the values for all
flags you want set on the instance; any flag not specifically included is
set to its default value. For flags that don't take a value, specify the
flag name followed by an equals sign ("=").</p>



<p>For example, to set the <code translate="no" dir="ltr">general_log</code>,
<code translate="no" dir="ltr">skip_show_database</code>, and <code translate="no" dir="ltr">wait_timeout</code> flags, you
can use the following command:</p>
<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="Bash">gcloud<span class="devsite-syntax-w"> </span>sql<span class="devsite-syntax-w"> </span>instances<span class="devsite-syntax-w"> </span>patch<span class="devsite-syntax-w"> </span><var translate="no">INSTANCE_NAME</var><span class="devsite-syntax-w"> </span><span class="devsite-syntax-se">\</span>
<span class="devsite-syntax-w">  </span>--database-flags<span class="devsite-syntax-o">=</span><span class="devsite-syntax-nv">general_log</span><span class="devsite-syntax-o">=</span>on,skip_show_database<span class="devsite-syntax-o">=</span>on,wait_timeout<span class="devsite-syntax-o">=</span><span class="devsite-syntax-m">200000</span></pre></devsite-code>



</section>

<section>
      <h3 id="terraform" data-text="Terraform" tabindex="-1">Terraform</h3>
      <p>To add database flags, use a <a href="https://registry.terraform.io/providers/hashicorp/google/latest/docs/resources/sql_database_instance"">Terraform resource</a>.
      </p>

      
      






  
  














  



<div class="github-docwidget-gitinclude-code">

  
    
  
  



















  




  



  


  <div></div><devsite-code><pre suppresswarning="suppresswarning" translate="no" class="lang-terraform devsite-click-to-copy"
       track-metadata-position="terraform-google-modules/terraform-docs-samples/cloud_sql/mysql_instance_flags/main.tf/main/cloud_sql_mysql_instance_flags"
       
       data-code-snippet="true"
       
       data-github-includecode-link="https://github.com/terraform-google-modules/terraform-docs-samples/blob/main/cloud_sql/mysql_instance_flags/main.tf"
       track-metadata-snippet-file-url="https://github.com/terraform-google-modules/terraform-docs-samples/blob/main/cloud_sql/mysql_instance_flags/main.tf"
       
       
       feedback-context="{&#34;language&#34;: &#34;terraform&#34;, &#34;region_tag&#34;: &#34;cloud_sql_mysql_instance_flags&#34;, &#34;snippet_file_url&#34;: &#34;https://github.com/terraform-google-modules/terraform-docs-samples/blob/main/cloud_sql/mysql_instance_flags/main.tf&#34;}"
       feedback-product="1634365"
       feedback-bucket="storage"
       
       
       language="terraform"
       
       
       
       
       data-github-path="terraform-google-modules/terraform-docs-samples/cloud_sql/mysql_instance_flags/main.tf"
       
       
       data-git-revision="main"
       
       
       data-region-tag="cloud_sql_mysql_instance_flags"
       track-metadata-region-tag="cloud_sql_mysql_instance_flags"
       
       
       
        dir="ltr" is-upgraded><code translate="no" dir="ltr">resource &quot;google_sql_database_instance&quot; &quot;instance&quot; {
  database_version = &quot;MYSQL_8_0&quot;
  name             = &quot;mysql-instance&quot;
  region           = &quot;us-central1&quot;
  settings {
    database_flags {
      name  = &quot;general_log&quot;
      value = &quot;on&quot;
    }
    database_flags {
      name  = &quot;skip_show_database&quot;
      value = &quot;on&quot;
    }
    database_flags {
      name  = &quot;wait_timeout&quot;
      value = &quot;200000&quot;
    }
    disk_type = &quot;PD_SSD&quot;
    tier      = &quot;db-n1-standard-2&quot;
  }
  # set `deletion_protection` to true, will ensure that one cannot accidentally delete this instance by
  # use of Terraform whereas `deletion_protection_enabled` flag protects this instance at the GCP level.
  deletion_protection = false
}</code></pre></devsite-code>
</div>



























      

      

      

       <h4 id="apply-the-changes" data-text="Apply the changes" tabindex="-1">Apply the changes</h4>

        <p>To apply your Terraform configuration in a Google Cloud project, complete the steps in the
   following sections.</p>
<h2 id="prepare-cloud-shell" data-text="Prepare Cloud Shell" tabindex="-1">Prepare Cloud Shell</h2>
<ol>
  <li>Launch <a href="https://shell.cloud.google.com/">Cloud Shell</a>.</li>
  <li>
    <p>Set the default Google Cloud project
      where you want to apply your Terraform configurations.
    </p>
    <p>You only need to run this command once per project, and you can run it in any directory.</p>
    <div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded>export GOOGLE_CLOUD_PROJECT=<var translate="no">PROJECT_ID</var></pre></devsite-code>
    <p>Environment variables are overridden if you set explicit values in the Terraform
      configuration file.</p>
  </li>
</ol>
<h2 id="prepare-the-directory" data-text="Prepare the directory" tabindex="-1">Prepare the directory</h2>
<p>Each Terraform configuration file must have its own directory (also
called a <em>root module</em>).</p>
<ol>
  <li>
    In <a href="https://shell.cloud.google.com/">Cloud Shell</a>, create a directory and a new
    file within that directory. The filename must have the
    <code translate="no" dir="ltr">.tf</code> extension&mdash;for example <code translate="no" dir="ltr">main.tf</code>. In this
    tutorial, the file is referred to as <code translate="no" dir="ltr">main.tf</code>.
    <div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded>mkdir <var translate="no">DIRECTORY</var> && cd <var translate="no">DIRECTORY</var> && touch main.tf</pre></devsite-code>
  </li>
  <li>
    <p>If you are following a tutorial, you can copy the sample code in each section or step.</p>
    <p>Copy the sample code into the newly created <code translate="no" dir="ltr">main.tf</code>.</p>
    <p>Optionally, copy the code from GitHub. This is recommended
      when the Terraform snippet is part of an end-to-end solution.
    </p>
  </li>
  <li>Review and modify the sample parameters to apply to your environment.</li>
  <li>Save your changes.</li>
  <li>
    Initialize Terraform. You only need to do this once per directory.
    <div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded>terraform init</pre></devsite-code>
    <p>Optionally, to use the latest Google provider version, include the <code translate="no" dir="ltr">-upgrade</code>
      option:
    </p>
    <div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded>terraform init -upgrade</pre></devsite-code>
  </li>
</ol>
<h2 id="apply-the-changes_1" data-text="Apply the changes" tabindex="-1">Apply the changes</h2>
<ol>
  <li>
    Review the configuration and verify that the resources that Terraform is going to create or
    update match your expectations:
    <div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded>terraform plan</pre></devsite-code>
    <p>Make corrections to the configuration as necessary.</p>
  </li>
  <li>
    Apply the Terraform configuration by running the following command and entering <code translate="no" dir="ltr">yes</code>
    at the prompt:
    <div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded>terraform apply</pre></devsite-code>
    <p>Wait until Terraform displays the "Apply complete!" message.</p>
  </li>
  <li><a href="https://console.cloud.google.com/">Open your Google Cloud project</a> to view
    the results. In the Google Cloud console, navigate to your resources in the UI to make sure
    that Terraform has created or updated them.
  </li>
</ol>
<aside class="note"><b>Note:</b> Terraform samples typically assume that the required APIs are
  enabled in your Google Cloud project.
</aside>

      <h4 id="delete-the-changes" data-text="Delete the changes" tabindex="-1">Delete the changes</h4>

                <p>To delete your changes, do the following:</p>

         <ol>
            <li>To disable deletion protection, in your Terraform configuration file set the
            <code translate="no" dir="ltr">deletion_protection</code> argument to <code translate="no" dir="ltr">false</code>.

              <div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded>deletion_protection =  "false"</pre></devsite-code>
            </li>

            <li>Apply the updated Terraform configuration by running the following command and
            entering <code translate="no" dir="ltr">yes</code> at the prompt:

              <div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded>terraform apply</pre></devsite-code>
            </li>
         </ol>

         <ol start="3">

            <li>

             <p>Remove resources previously applied with your Terraform configuration by running the following
   command and entering <code translate="no" dir="ltr">yes</code> at the prompt:</p>
<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded>terraform destroy</pre></devsite-code>

            </li>
          </ol>
</section>

<section>
<h3 id="rest-v1" data-text="REST v1" tabindex="-1">REST v1</h3>
<p>To set a flag for an existing database:</p>
  










  





  





  







  
    
    
  




  




  
    
  


















<p>
  Before using any of the request data,
  make the following replacements:
</p>

<ul>
  <li><var translate="no">project-id</var>: The project ID</li>
  <li><var translate="no">instance-id</var>: The instance ID</li>
</ul>




<p>
  HTTP method and URL:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="HTTP method and URL" translate="no" dir="ltr" is-upgraded>PATCH https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var></pre></devsite-code>
</section>



<p>
  Request JSON body:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="request body" translate="no" dir="ltr" is-upgraded>
{
  "settings":
  {
    "databaseFlags":
    [
      {
        "name": "<var translate="no">flag_name</var>",
        "value": "<var translate="no">flag_value</var>"
      }
    ]
  }
}
</pre></devsite-code>
</section>




<p>To send your request, expand one of these options:</p>




<section class="expandable" >
  <h4 class="showalways" id="curl-linux,-macos,-or-cloud-shell" data-text="curl (Linux, macOS, or Cloud Shell)" tabindex="-1">curl (Linux, macOS, or Cloud Shell)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            
            , or by using <a href="/shell/docs">Cloud Shell</a>,
            which automatically logs you into the <code translate="no" dir="ltr">gcloud</code> CLI
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  

  
  

  
  

  
  
    
  

  
  
    
  

  
  

  
  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label=" CURL command" translate="no" dir="ltr" is-upgraded>curl -X PATCH \<br>     -H "Authorization: Bearer $(gcloud auth print-access-token)" \<br>     -H "Content-Type: application/json; charset=utf-8" \<br>     -d @request.json \<br>     "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>"</pre></devsite-code></section>
</section>



<section class="expandable" >
  <h4 class="showalways" id="powershell-windows" data-text="PowerShell (Windows)" tabindex="-1">PowerShell (Windows)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  
    
    
  

  
  

  
  

  
  

  
  

  

  
  
    
  

  
  
    
  

  
  
    
  

  
  
  
    
  

  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label="PowerShell command" translate="no" dir="ltr" is-upgraded>$cred = gcloud auth print-access-token<br>$headers = @{ "Authorization" = "Bearer $cred" }<br><br>Invoke-WebRequest `<br>    -Method PATCH `<br>    -Headers $headers `<br>    -ContentType: "application/json; charset=utf-8" `<br>    -InFile request.json `<br>    -Uri "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>" | Select-Object -Expand Content</pre></devsite-code></section>
</section>











    <p>You should receive a JSON response similar to the following:</p>
    


<section class="expandable"><h4 class="showalways" id="response" data-text="Response" tabindex="-1">Response</h4>

  <div></div><devsite-code><pre class="readonly" data-label="sample response" translate="no" dir="ltr" is-upgraded>
{
  "kind": "sql#operation",
  "targetLink": "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>",
  "status": "PENDING",
  "user": "user@example.com",
  "insertTime": "2020-01-21T22:43:37.981Z",
  "operationType": "UPDATE",
  "name": "<var translate="no">operation-id</var>",
  "targetId": "<var translate="no">instance-id</var>",
  "selfLink": "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/operations/<var translate="no">operation-id</var>",
  "targetProject": "<var translate="no">project-id</var>"
}
</pre></devsite-code>
</section>































<p>For example, to set the <code translate="no" dir="ltr">general_log</code> flag for an existing
database use:</p>
  










  





  





  







  
    
    
  




  




  
    
  


















<p>
  Before using any of the request data,
  make the following replacements:
</p>

<ul>
  <li><var translate="no">project-id</var>: The project ID</li>
  <li><var translate="no">instance-id</var>: The instance ID</li>
</ul>




<p>
  HTTP method and URL:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="HTTP method and URL" translate="no" dir="ltr" is-upgraded>PATCH https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var></pre></devsite-code>
</section>



<p>
  Request JSON body:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="request body" translate="no" dir="ltr" is-upgraded>
{
  "settings":
  {
    "databaseFlags":
    [
      {
        "name": "general_log",
        "value": "on"
      }
    ]
  }
}
</pre></devsite-code>
</section>




<p>To send your request, expand one of these options:</p>




<section class="expandable" >
  <h4 class="showalways" id="curl-linux,-macos,-or-cloud-shell_1" data-text="curl (Linux, macOS, or Cloud Shell)" tabindex="-1">curl (Linux, macOS, or Cloud Shell)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            
            , or by using <a href="/shell/docs">Cloud Shell</a>,
            which automatically logs you into the <code translate="no" dir="ltr">gcloud</code> CLI
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  

  
  

  
  

  
  
    
  

  
  
    
  

  
  

  
  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label=" CURL command" translate="no" dir="ltr" is-upgraded>curl -X PATCH \<br>     -H "Authorization: Bearer $(gcloud auth print-access-token)" \<br>     -H "Content-Type: application/json; charset=utf-8" \<br>     -d @request.json \<br>     "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>"</pre></devsite-code></section>
</section>



<section class="expandable" >
  <h4 class="showalways" id="powershell-windows_1" data-text="PowerShell (Windows)" tabindex="-1">PowerShell (Windows)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  
    
    
  

  
  

  
  

  
  

  
  

  

  
  
    
  

  
  
    
  

  
  
    
  

  
  
  
    
  

  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label="PowerShell command" translate="no" dir="ltr" is-upgraded>$cred = gcloud auth print-access-token<br>$headers = @{ "Authorization" = "Bearer $cred" }<br><br>Invoke-WebRequest `<br>    -Method PATCH `<br>    -Headers $headers `<br>    -ContentType: "application/json; charset=utf-8" `<br>    -InFile request.json `<br>    -Uri "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>" | Select-Object -Expand Content</pre></devsite-code></section>
</section>











    <p>You should receive a JSON response similar to the following:</p>
    


<section class="expandable"><h4 class="showalways" id="response_1" data-text="Response" tabindex="-1">Response</h4>

  <div></div><devsite-code><pre class="readonly" data-label="sample response" translate="no" dir="ltr" is-upgraded>
{
  "kind": "sql#operation",
  "targetLink": "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>",
  "status": "PENDING",
  "user": "user@example.com",
  "insertTime": "2020-01-21T22:43:37.981Z",
  "operationType": "UPDATE",
  "name": "<var translate="no">operation-id</var>",
  "targetId": "<var translate="no">instance-id</var>",
  "selfLink": "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/operations/<var translate="no">operation-id</var>",
  "targetProject": "<var translate="no">project-id</var>"
}
</pre></devsite-code>
</section>































<p>If there are existing flags configured for the database, modify the previous
command to include them. The <code translate="no" dir="ltr">PATCH</code> command overwrites the existing
flags with the ones specified in the request.
</p>
</section>

<section>
<h3 id="rest-v1beta4" data-text="REST v1beta4" tabindex="-1">REST v1beta4</h3>
<p>To set a flag for an existing database:</p>
  










  





  





  







  
    
    
  




  




  
    
  


















<p>
  Before using any of the request data,
  make the following replacements:
</p>

<ul>
  <li><var translate="no">project-id</var>: The project ID</li>
  <li><var translate="no">instance-id</var>: The instance ID</li>
</ul>




<p>
  HTTP method and URL:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="HTTP method and URL" translate="no" dir="ltr" is-upgraded>PATCH https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var></pre></devsite-code>
</section>



<p>
  Request JSON body:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="request body" translate="no" dir="ltr" is-upgraded>
{
  "settings":
  {
    "databaseFlags":
    [
      {
        "name": "<var translate="no">flag_name</var>",
        "value": "<var translate="no">flag_value</var>"
      }
    ]
  }
}
</pre></devsite-code>
</section>




<p>To send your request, expand one of these options:</p>




<section class="expandable" >
  <h4 class="showalways" id="curl-linux,-macos,-or-cloud-shell_2" data-text="curl (Linux, macOS, or Cloud Shell)" tabindex="-1">curl (Linux, macOS, or Cloud Shell)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            
            , or by using <a href="/shell/docs">Cloud Shell</a>,
            which automatically logs you into the <code translate="no" dir="ltr">gcloud</code> CLI
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  

  
  

  
  

  
  
    
  

  
  
    
  

  
  

  
  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label=" CURL command" translate="no" dir="ltr" is-upgraded>curl -X PATCH \<br>     -H "Authorization: Bearer $(gcloud auth print-access-token)" \<br>     -H "Content-Type: application/json; charset=utf-8" \<br>     -d @request.json \<br>     "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>"</pre></devsite-code></section>
</section>



<section class="expandable" >
  <h4 class="showalways" id="powershell-windows_2" data-text="PowerShell (Windows)" tabindex="-1">PowerShell (Windows)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  
    
    
  

  
  

  
  

  
  

  
  

  

  
  
    
  

  
  
    
  

  
  
    
  

  
  
  
    
  

  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label="PowerShell command" translate="no" dir="ltr" is-upgraded>$cred = gcloud auth print-access-token<br>$headers = @{ "Authorization" = "Bearer $cred" }<br><br>Invoke-WebRequest `<br>    -Method PATCH `<br>    -Headers $headers `<br>    -ContentType: "application/json; charset=utf-8" `<br>    -InFile request.json `<br>    -Uri "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>" | Select-Object -Expand Content</pre></devsite-code></section>
</section>











    <p>You should receive a JSON response similar to the following:</p>
    


<section class="expandable"><h4 class="showalways" id="response_2" data-text="Response" tabindex="-1">Response</h4>

  <div></div><devsite-code><pre class="readonly" data-label="sample response" translate="no" dir="ltr" is-upgraded>
{
  "kind": "sql#operation",
  "targetLink": "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>",
  "status": "PENDING",
  "user": "user@example.com",
  "insertTime": "2020-01-21T22:43:37.981Z",
  "operationType": "UPDATE",
  "name": "<var translate="no">operation-id</var>",
  "targetId": "<var translate="no">instance-id</var>",
  "selfLink": "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/operations/<var translate="no">operation-id</var>",
  "targetProject": "<var translate="no">project-id</var>"
}
</pre></devsite-code>
</section>































<p>For example, to set the <code translate="no" dir="ltr">general_log</code> flag for an existing
database use:</p>
  










  





  





  







  
    
    
  




  




  
    
  


















<p>
  Before using any of the request data,
  make the following replacements:
</p>

<ul>
  <li><var translate="no">project-id</var>: The project ID</li>
  <li><var translate="no">instance-id</var>: The instance ID</li>
</ul>




<p>
  HTTP method and URL:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="HTTP method and URL" translate="no" dir="ltr" is-upgraded>PATCH https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var></pre></devsite-code>
</section>



<p>
  Request JSON body:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="request body" translate="no" dir="ltr" is-upgraded>
{
  "settings":
  {
    "databaseFlags":
    [
      {
        "name": "general_log",
        "value": "on"
      }
    ]
  }
}
</pre></devsite-code>
</section>




<p>To send your request, expand one of these options:</p>




<section class="expandable" >
  <h4 class="showalways" id="curl-linux,-macos,-or-cloud-shell_3" data-text="curl (Linux, macOS, or Cloud Shell)" tabindex="-1">curl (Linux, macOS, or Cloud Shell)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            
            , or by using <a href="/shell/docs">Cloud Shell</a>,
            which automatically logs you into the <code translate="no" dir="ltr">gcloud</code> CLI
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  

  
  

  
  

  
  
    
  

  
  
    
  

  
  

  
  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label=" CURL command" translate="no" dir="ltr" is-upgraded>curl -X PATCH \<br>     -H "Authorization: Bearer $(gcloud auth print-access-token)" \<br>     -H "Content-Type: application/json; charset=utf-8" \<br>     -d @request.json \<br>     "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>"</pre></devsite-code></section>
</section>



<section class="expandable" >
  <h4 class="showalways" id="powershell-windows_3" data-text="PowerShell (Windows)" tabindex="-1">PowerShell (Windows)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  
    
    
  

  
  

  
  

  
  

  
  

  

  
  
    
  

  
  
    
  

  
  
    
  

  
  
  
    
  

  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label="PowerShell command" translate="no" dir="ltr" is-upgraded>$cred = gcloud auth print-access-token<br>$headers = @{ "Authorization" = "Bearer $cred" }<br><br>Invoke-WebRequest `<br>    -Method PATCH `<br>    -Headers $headers `<br>    -ContentType: "application/json; charset=utf-8" `<br>    -InFile request.json `<br>    -Uri "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>" | Select-Object -Expand Content</pre></devsite-code></section>
</section>











    <p>You should receive a JSON response similar to the following:</p>
    


<section class="expandable"><h4 class="showalways" id="response_3" data-text="Response" tabindex="-1">Response</h4>

  <div></div><devsite-code><pre class="readonly" data-label="sample response" translate="no" dir="ltr" is-upgraded>
{
  "kind": "sql#operation",
  "targetLink": "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>",
  "status": "PENDING",
  "user": "user@example.com",
  "insertTime": "2020-01-21T22:43:37.981Z",
  "operationType": "UPDATE",
  "name": "<var translate="no">operation-id</var>",
  "targetId": "<var translate="no">instance-id</var>",
  "selfLink": "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/operations/<var translate="no">operation-id</var>",
  "targetProject": "<var translate="no">project-id</var>"
}
</pre></devsite-code>
</section>































<p>If there are existing flags configured for the database, modify the previous
command to include them. The <code translate="no" dir="ltr">PATCH</code> command overwrites the existing
flags with the ones specified in the request.
</p>
</section>

</div>

<h3 id="clear_all_flags_to_their_default_values" data-text="Clear all flags to their default values" tabindex="-1">Clear all flags to their default values</h3>

<div id="set-flag" class="ds-selector-tabs" data-ds-scope="create-instance">
<section>

<h3 id="console_1" data-text="Console" tabindex="-1">Console</h3>

<ol>
      <li>In the <a href="https://console.cloud.google.com/project/_/sql/instances">Google Cloud console</a>,
select the project that contains the Cloud SQL instance for which you want to clear all flags.</li>
  <li>Open the instance and click <b>Edit</b>.</li>
  <li>Open the <b>Database flags</b> section.</li>
  <li>Click the <b>X</b> next to all of the flags shown.</li>
  <li>Click <b>Save</b> to save your changes.</li>
</ol>

</section>

<section>

<h3 id="gcloud_1" data-text="gcloud" tabindex="-1">gcloud</h3>

<p>Clear all flags to their default values on an instance:</p>
<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="Bash">gcloud<span class="devsite-syntax-w"> </span>sql<span class="devsite-syntax-w"> </span>instances<span class="devsite-syntax-w"> </span>patch<span class="devsite-syntax-w"> </span><var translate="no">INSTANCE_NAME</var><span class="devsite-syntax-w"> </span><span class="devsite-syntax-se">\</span>
--clear-database-flags</pre></devsite-code>
<p>You are prompted to confirm that the instance will be restarted.</p>
</section>

<section>
<h3 id="rest-v1_1" data-text="REST v1" tabindex="-1">REST v1</h3>
  <p>To clear all flags for an existing instance:</p>
  










  





  





  







  
    
    
  




  




  
    
  


















<p>
  Before using any of the request data,
  make the following replacements:
</p>

<ul>
  <li><var translate="no">project-id</var>: The project ID</li>
  <li><var translate="no">instance-id</var>: The instance ID</li>
</ul>




<p>
  HTTP method and URL:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="HTTP method and URL" translate="no" dir="ltr" is-upgraded>PATCH https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var></pre></devsite-code>
</section>



<p>
  Request JSON body:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="request body" translate="no" dir="ltr" is-upgraded>
{
  "settings":
  {
    "databaseFlags": []
  }
}
</pre></devsite-code>
</section>




<p>To send your request, expand one of these options:</p>




<section class="expandable" >
  <h4 class="showalways" id="curl-linux,-macos,-or-cloud-shell_4" data-text="curl (Linux, macOS, or Cloud Shell)" tabindex="-1">curl (Linux, macOS, or Cloud Shell)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            
            , or by using <a href="/shell/docs">Cloud Shell</a>,
            which automatically logs you into the <code translate="no" dir="ltr">gcloud</code> CLI
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  

  
  

  
  

  
  
    
  

  
  
    
  

  
  

  
  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label=" CURL command" translate="no" dir="ltr" is-upgraded>curl -X PATCH \<br>     -H "Authorization: Bearer $(gcloud auth print-access-token)" \<br>     -H "Content-Type: application/json; charset=utf-8" \<br>     -d @request.json \<br>     "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>"</pre></devsite-code></section>
</section>



<section class="expandable" >
  <h4 class="showalways" id="powershell-windows_4" data-text="PowerShell (Windows)" tabindex="-1">PowerShell (Windows)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  
    
    
  

  
  

  
  

  
  

  
  

  

  
  
    
  

  
  
    
  

  
  
    
  

  
  
  
    
  

  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label="PowerShell command" translate="no" dir="ltr" is-upgraded>$cred = gcloud auth print-access-token<br>$headers = @{ "Authorization" = "Bearer $cred" }<br><br>Invoke-WebRequest `<br>    -Method PATCH `<br>    -Headers $headers `<br>    -ContentType: "application/json; charset=utf-8" `<br>    -InFile request.json `<br>    -Uri "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>" | Select-Object -Expand Content</pre></devsite-code></section>
</section>











    <p>You should receive a JSON response similar to the following:</p>
    


<section class="expandable"><h4 class="showalways" id="response_4" data-text="Response" tabindex="-1">Response</h4>

  <div></div><devsite-code><pre class="readonly" data-label="sample response" translate="no" dir="ltr" is-upgraded>
{
  "kind": "sql#operation",
  "targetLink": "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>",
  "status": "PENDING",
  "user": "user@example.com",
  "insertTime": "2020-01-21T22:43:37.981Z",
  "operationType": "UPDATE",
  "name": "<var translate="no">operation-id</var>",
  "targetId": "<var translate="no">instance-id</var>",
  "selfLink": "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/operations/<var translate="no">operation-id</var>",
  "targetProject": "<var translate="no">project-id</var>"
}
</pre></devsite-code>
</section>




























</section>

<section>
<h3 id="rest-v1beta4_1" data-text="REST v1beta4" tabindex="-1">REST v1beta4</h3>
  <p>To clear all flags for an existing instance:</p>
  










  





  





  







  
    
    
  




  




  
    
  


















<p>
  Before using any of the request data,
  make the following replacements:
</p>

<ul>
  <li><var translate="no">project-id</var>: The project ID</li>
  <li><var translate="no">instance-id</var>: The instance ID</li>
</ul>




<p>
  HTTP method and URL:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="HTTP method and URL" translate="no" dir="ltr" is-upgraded>PATCH https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var></pre></devsite-code>
</section>



<p>
  Request JSON body:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="request body" translate="no" dir="ltr" is-upgraded>
{
  "settings":
  {
    "databaseFlags": []
  }
}
</pre></devsite-code>
</section>




<p>To send your request, expand one of these options:</p>




<section class="expandable" >
  <h4 class="showalways" id="curl-linux,-macos,-or-cloud-shell_5" data-text="curl (Linux, macOS, or Cloud Shell)" tabindex="-1">curl (Linux, macOS, or Cloud Shell)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            
            , or by using <a href="/shell/docs">Cloud Shell</a>,
            which automatically logs you into the <code translate="no" dir="ltr">gcloud</code> CLI
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  

  
  

  
  

  
  
    
  

  
  
    
  

  
  

  
  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label=" CURL command" translate="no" dir="ltr" is-upgraded>curl -X PATCH \<br>     -H "Authorization: Bearer $(gcloud auth print-access-token)" \<br>     -H "Content-Type: application/json; charset=utf-8" \<br>     -d @request.json \<br>     "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>"</pre></devsite-code></section>
</section>



<section class="expandable" >
  <h4 class="showalways" id="powershell-windows_5" data-text="PowerShell (Windows)" tabindex="-1">PowerShell (Windows)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
    <p>
      Save the request body in a file named <code translate="no" dir="ltr">request.json</code>,
      and execute the following command:
    </p>
    

  

  
  
    
  

  
  

  
  
    
    
  

  
  

  
  

  
  

  
  

  

  
  
    
  

  
  
    
  

  
  
    
  

  
  
  
    
  

  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label="PowerShell command" translate="no" dir="ltr" is-upgraded>$cred = gcloud auth print-access-token<br>$headers = @{ "Authorization" = "Bearer $cred" }<br><br>Invoke-WebRequest `<br>    -Method PATCH `<br>    -Headers $headers `<br>    -ContentType: "application/json; charset=utf-8" `<br>    -InFile request.json `<br>    -Uri "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>" | Select-Object -Expand Content</pre></devsite-code></section>
</section>











    <p>You should receive a JSON response similar to the following:</p>
    


<section class="expandable"><h4 class="showalways" id="response_5" data-text="Response" tabindex="-1">Response</h4>

  <div></div><devsite-code><pre class="readonly" data-label="sample response" translate="no" dir="ltr" is-upgraded>
{
  "kind": "sql#operation",
  "targetLink": "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>",
  "status": "PENDING",
  "user": "user@example.com",
  "insertTime": "2020-01-21T22:43:37.981Z",
  "operationType": "UPDATE",
  "name": "<var translate="no">operation-id</var>",
  "targetId": "<var translate="no">instance-id</var>",
  "selfLink": "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/operations/<var translate="no">operation-id</var>",
  "targetProject": "<var translate="no">project-id</var>"
}
</pre></devsite-code>
</section>




























</section>

</div>



<h3 id="view_current_values_of_database_flags" data-text="View current values of database flags" tabindex="-1">View current values of database flags</h3>


To view all current values of the MySQL system variables, log into
your instance with the <code translate="no" dir="ltr">mysql</code> client and enter the following statement:</p>
<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="SQL"><code translate="no" dir="ltr"><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">SHOW</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-n">VARIABLES</span><span class="devsite-syntax-p">;</span>
</code></pre></devsite-code>
<p>Note that you can change the value only for supported flags (as listed below).</p>




<h3 id="determine_which_database_flags_have_been_set_for_an_instance" data-text="Determine which database flags have been set for an instance" tabindex="-1">Determine which database flags have been set for an instance</h3>

<p>To see which flags have been set for a Cloud SQL instance:</p>

<p><div id="set-flag" class="ds-selector-tabs" data-ds-scope="create-instance">
  <section>
  <h3 id="console_2" data-text="Console" tabindex="-1">Console</h3>
  <ol>
         <li>In the <a href="https://console.cloud.google.com/project/_/sql/instances">Google Cloud console</a>,
select the project that contains the Cloud SQL instance for which you want to see the database flags that have been set.</li>
    <li>Select the instance to open its <b>Instance Overview</b> page.
    <p>The database flags that have been set are listed under the
      <b>Database flags</b> section.</p></li>
    </ol>
    </section>
    <section>
  <h3 id="gcloud_2" data-text="gcloud" tabindex="-1">gcloud</h3>
  <p>Get the instance state:</p>
<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="Bash">gcloud<span class="devsite-syntax-w"> </span>sql<span class="devsite-syntax-w"> </span>instances<span class="devsite-syntax-w"> </span>describe<span class="devsite-syntax-w"> </span><var translate="no">INSTANCE_NAME</var></pre></devsite-code>
<p>In the output, database flags are listed under the <code translate="no" dir="ltr">settings</code> as
the collection <code translate="no" dir="ltr">databaseFlags</code>. For more information
  about the representation of the flags in the output, see
  <a href="/sql/docs/mysql/admin-api/rest/v1beta4/instances">Instances Resource Representation</a>.
  </p>
</section></p>

<section>
<h3 id="rest-v1_2" data-text="REST v1" tabindex="-1">REST v1</h3>
<p>To list flags configured for an instance:</p>















  





  







  
  




  




  
    
  


















<p>
  Before using any of the request data,
  make the following replacements:
</p>

<ul>
  <li><var translate="no">project-id</var>: The project ID</li>
  <li><var translate="no">instance-id</var>: The instance ID</li>
</ul>




<p>
  HTTP method and URL:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="HTTP method and URL" translate="no" dir="ltr" is-upgraded>GET https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var></pre></devsite-code>
</section>






<p>To send your request, expand one of these options:</p>




<section class="expandable" >
  <h4 class="showalways" id="curl-linux,-macos,-or-cloud-shell_6" data-text="curl (Linux, macOS, or Cloud Shell)" tabindex="-1">curl (Linux, macOS, or Cloud Shell)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            
            , or by using <a href="/shell/docs">Cloud Shell</a>,
            which automatically logs you into the <code translate="no" dir="ltr">gcloud</code> CLI
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
  <p>
    Execute the following command:
  </p>
  

  

  
  
    
  

  
  

  
  

  
  

  
  

  
  

  
  

  
  

  
  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label=" CURL command" translate="no" dir="ltr" is-upgraded>curl -X GET \<br>     -H "Authorization: Bearer $(gcloud auth print-access-token)" \<br>     "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>"</pre></devsite-code></section>
</section>



<section class="expandable" >
  <h4 class="showalways" id="powershell-windows_6" data-text="PowerShell (Windows)" tabindex="-1">PowerShell (Windows)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
  <p>
    Execute the following command:
  </p>
  

  

  
  
    
  

  
  

  
  
    
    
  

  
  

  
  

  
  

  
  

  

  
  
    
  

  
  

  
  

  
  
  
    
  

  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label="PowerShell command" translate="no" dir="ltr" is-upgraded>$cred = gcloud auth print-access-token<br>$headers = @{ "Authorization" = "Bearer $cred" }<br><br>Invoke-WebRequest `<br>    -Method GET `<br>    -Headers $headers `<br>    -Uri "https://sqladmin.googleapis.com/v1/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>" | Select-Object -Expand Content</pre></devsite-code></section>
</section>











    <p>You should receive a JSON response similar to the following:</p>
    


<section class="expandable"><h4 class="showalways" id="response_6" data-text="Response" tabindex="-1">Response</h4>

  <div></div><devsite-code><pre class="readonly" data-label="sample response" translate="no" dir="ltr" is-upgraded>
{
  "settings":
  {
    "authorizedGaeApplications": [],
    "tier": "<var translate="no">machine-type</var>",
    "kind": "sql#settings",
    "availabilityType": "REGIONAL",
    "pricingPlan": "PER_USE",
    "replicationType": "SYNCHRONOUS",
    "activationPolicy": "ALWAYS",
    "ipConfiguration":
    {
      "privateNetwork": "projects/<var translate="no">project-id</var>/global/networks/default",
      "authorizedNetworks": [],
      "ipv4Enabled": false
    },
    "locationPreference":
    {
      "zone": "<var translate="no">zone</var>",
      "kind": "sql#locationPreference"
    },
    "databaseFlags": [
      {
        "name": "general_log",
        "value": "on"
      }
    ],
    "dataDiskType": "PD_SSD",
    "maintenanceWindow":
    {
      "kind": "sql#maintenanceWindow",
      "hour": 0,
      "day": 0
    },
    "backupConfiguration":
    {
      "startTime": "03:00",
      "kind": "sql#backupConfiguration",
      "enabled": true,
      "binaryLogEnabled": true
    },
    "settingsVersion": "54",
    "storageAutoResizeLimit": "0",
    "storageAutoResize": true,
    "dataDiskSizeGb": "10"
  }
}
</pre></devsite-code>
</section>




























<p>In the output, look for the <code translate="no" dir="ltr">databaseFlags</code> field.</p>
</section>

<section>
<h3 id="rest-v1beta4_2" data-text="REST v1beta4" tabindex="-1">REST v1beta4</h3>
<p>To list flags configured for an instance:</p>















  





  







  
  




  




  
    
  


















<p>
  Before using any of the request data,
  make the following replacements:
</p>

<ul>
  <li><var translate="no">project-id</var>: The project ID</li>
  <li><var translate="no">instance-id</var>: The instance ID</li>
</ul>




<p>
  HTTP method and URL:
</p>
<section>
  <div></div><devsite-code><pre class="devsite-click-to-copy" data-label="HTTP method and URL" translate="no" dir="ltr" is-upgraded>GET https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var></pre></devsite-code>
</section>






<p>To send your request, expand one of these options:</p>




<section class="expandable" >
  <h4 class="showalways" id="curl-linux,-macos,-or-cloud-shell_7" data-text="curl (Linux, macOS, or Cloud Shell)" tabindex="-1">curl (Linux, macOS, or Cloud Shell)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            
            , or by using <a href="/shell/docs">Cloud Shell</a>,
            which automatically logs you into the <code translate="no" dir="ltr">gcloud</code> CLI
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
  <p>
    Execute the following command:
  </p>
  

  

  
  
    
  

  
  

  
  

  
  

  
  

  
  

  
  

  
  

  
  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label=" CURL command" translate="no" dir="ltr" is-upgraded>curl -X GET \<br>     -H "Authorization: Bearer $(gcloud auth print-access-token)" \<br>     "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>"</pre></devsite-code></section>
</section>



<section class="expandable" >
  <h4 class="showalways" id="powershell-windows_7" data-text="PowerShell (Windows)" tabindex="-1">PowerShell (Windows)</h4>

  
  
    
      <aside class="note"><b>Note:</b>
        
          The following command assumes that you have logged in to
          the <code translate="no" dir="ltr">gcloud</code> CLI with your user account by running
          <a href="/sdk/gcloud/reference/init"><code translate="no" dir="ltr">gcloud init</code></a>
          or
          <a href="/sdk/gcloud/reference/auth/login"><code translate="no" dir="ltr">gcloud auth login</code></a>
            .
          You can check the currently active account by running
          <a href="/sdk/gcloud/reference/auth/list"><code translate="no" dir="ltr">gcloud auth list</code></a>.
        
      </aside>
    
  

  
  <p>
    Execute the following command:
  </p>
  

  

  
  
    
  

  
  

  
  
    
    
  

  
  

  
  

  
  

  
  

  

  
  
    
  

  
  

  
  

  
  
  
    
  

  <section><div></div><devsite-code><pre class="devsite-click-to-copy" data-label="PowerShell command" translate="no" dir="ltr" is-upgraded>$cred = gcloud auth print-access-token<br>$headers = @{ "Authorization" = "Bearer $cred" }<br><br>Invoke-WebRequest `<br>    -Method GET `<br>    -Headers $headers `<br>    -Uri "https://sqladmin.googleapis.com/sql/v1beta4/projects/<var translate="no">project-id</var>/instances/<var translate="no">instance-id</var>" | Select-Object -Expand Content</pre></devsite-code></section>
</section>











    <p>You should receive a JSON response similar to the following:</p>
    


<section class="expandable"><h4 class="showalways" id="response_7" data-text="Response" tabindex="-1">Response</h4>

  <div></div><devsite-code><pre class="readonly" data-label="sample response" translate="no" dir="ltr" is-upgraded>
{
  "settings":
  {
    "authorizedGaeApplications": [],
    "tier": "<var translate="no">machine-type</var>",
    "kind": "sql#settings",
    "availabilityType": "REGIONAL",
    "pricingPlan": "PER_USE",
    "replicationType": "SYNCHRONOUS",
    "activationPolicy": "ALWAYS",
    "ipConfiguration":
    {
      "privateNetwork": "projects/<var translate="no">project-id</var>/global/networks/default",
      "authorizedNetworks": [],
      "ipv4Enabled": false
    },
    "locationPreference":
    {
      "zone": "<var translate="no">zone</var>",
      "kind": "sql#locationPreference"
    },
    "databaseFlags": [
      {
        "name": "general_log",
        "value": "on"
      }
    ],
    "dataDiskType": "PD_SSD",
    "maintenanceWindow":
    {
      "kind": "sql#maintenanceWindow",
      "hour": 0,
      "day": 0
    },
    "backupConfiguration":
    {
      "startTime": "03:00",
      "kind": "sql#backupConfiguration",
      "enabled": true,
      "binaryLogEnabled": true
    },
    "settingsVersion": "54",
    "storageAutoResizeLimit": "0",
    "storageAutoResize": true,
    "dataDiskSizeGb": "10"
  }
}
</pre></devsite-code>
</section>




























<p>In the output, look for the <code translate="no" dir="ltr">databaseFlags</code> field.</p>
</section>

<p></div></p>

<h3 id="system-flags" data-text="Flags managed by Cloud SQL" tabindex="-1">Flags managed by Cloud SQL</h3>

<p>Cloud SQL adjusts certain system flags depending on the instance machine
type.</p>

<dl>
<dt id="buffer-instances"><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_buffer_pool_instances" target="_blank">innodb_buffer_pool_instances</a></dt>
<dd>
 <ul>
    <li> 1 for db-f1-micro and db-g1-small. </li>
    <li> 1 if RAM < 7.5 GB. </li>
    <li> 2 if 7.5 GB <= RAM < 13 GB. </li>
    <li> 4 if 13 GB <= RAM < 26 GB. </li>
    <li> 8 if RAM >= 26 GB. </li>
  </ul>
<p>
</dd>
</dl>

<h2 id="list-flags-mysql" data-text="Supported flags" tabindex="-1">Supported flags</h2>

<p>The flags supported in Cloud SQL are the most commonly requested flags for
MySQL. Flags not mentioned below are not supported.</p>

<p>
<p>For a given flag, Cloud SQL might support a different value or range
from the corresponding MySQL parameter or option.</p>
</p>

<p>The flags apply to all versions of MySQL supported by Cloud SQL
except where noted.</p>

<p><a href="#mysql-a">A</a> | <a href="#mysql-b">B</a> | <a href="#mysql-c">C</a> | <a href="#mysql-d">D</a> | <a href="#mysql-e">E</a> | <a href="#mysql-f">F</a> | <a href="#mysql-g">G</a> | <a href="#mysql-h">H</a> | <a href="#mysql-i">I</a> | <a href="#mysql-l">L</a> | <a href="#mysql-m">M</a> | <a href="#mysql-n">N</a> | <a href="#mysql-o">O</a> | <a href="#mysql-p">P</a> | <a href="#mysql-q">Q</a> | <a href="#mysql-r">R</a> | <a href="#mysql-s">S</a> | <a href="#mysql-t">T</a> | <a href="#mysql-u">U</a> | <a href="#mysql-w">W</a></p>

<table>
  <tr><th>Cloud SQL Flag</th>
      <th>Type<br>Acceptable Values and Notes</th>
      <th>Restart<br>Required?</th>
  </tr>
  <tr>
    <td><a id="mysql-a"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_activate_all_roles_on_login" target="_blank">activate_all_roles_on_login</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">off</code>
      <br>
     <aside class="note"><b>Note</b>: The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
    </td>
      <td>No</td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_autocommit" target="_blank">autocommit</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">on</code>
    </td>
      <td>No</td>
  </tr>
  <tr>
      <td><a id="mysql-a"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-source.html#sysvar_auto_increment_increment" target="_blank">auto_increment_increment</a></td>
      <td><code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 65535</code></td>
      <td>No</td>
  </tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-source.html#sysvar_auto_increment_offset" target="_blank">auto_increment_offset</a></td>
      <td> <code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 65535</code></td>
      <td>No</td>
  </tr>
    <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_automatic_sp_privileges" target="_blank">automatic_sp_privileges</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">on</code>
    </td>
      <td>No</td>
  </tr>
  <tr>
      <td><a id="mysql-b"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_back_log" target="_blank">back_log</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 65535</code>
      <br> default: <code translate="no" dir="ltr">max_connections</code>
      <br>
      <aside class="note"><b>Note:</b> The <code translate="no" dir="ltr">max_connections</code> flag
      enables the permitted backlog to
      adjust to the maximum permitted number of simultaneous client
      connections.</aside>
      </td>
      <td>Yes</td>
</tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_binlog_cache_size" target="_blank">binlog_cache_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">4096</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_binlog_expire_logs_seconds" target="_blank">binlog_expire_logs_seconds</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> or <code translate="no" dir="ltr">86400 (1 day)</code> ... <code translate="no" dir="ltr">4294967295 (max value)</code><br />Default is 86400, which equals 1 day.
        <p>See the
        <a href="/sql/docs/mysql/flags#tips-expire-logs" target="_blank">Tips</a>
        section for more information about this flag.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_binlog_group_commit_sync_delay" target="_blank">binlog_group_commit_sync_delay</a></td>
      <td><code translate="no" dir="ltr"></code>0 ... 1000000<code translate="no" dir="ltr"></code>
  <p>Supported in MySQL 5.7 and later</p>
  <p>Default is <b>0</b>.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_binlog_group_commit_sync_no_delay_count" target="_blank">binlog_group_commit_sync_no_delay_count</a></td>
      <td><code translate="no" dir="ltr"></code>0 ... 1000000<code translate="no" dir="ltr"></code>
        <p>Supported in MySQL 5.7 and later</p>
        <p>Default is <b>0</b>.</p></td>
      <td>No</td>
</tr>
<tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-gtids.html#sysvar_binlog_gtid_simple_recovery" target="_blank">binlog_gtid_simple_recovery</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">on</code>
    </td>
      <td>Yes</td>
</tr>
   <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_binlog_order_commits" target="_blank">binlog_order_commits</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">on</code>
   <p>See the
        <a href="/sql/docs/mysql/flags#tips-binlog-order">Tips</a>
        section for more information about this flag.</p></td>
    </td>
      <td>No</td>
  </tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_binlog_row_image" target="_blank">binlog_row_image</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">full</code> (default), <code translate="no" dir="ltr">minimal</code>, or <code translate="no" dir="ltr">noblob</code></td>
      <td>No</td>
</tr>
<tr>
     <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_binlog_row_metadata" target="_blank">binlog_row_metadata</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">full</code> or <code translate="no" dir="ltr">minimal</code> (default)</td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_binlog_row_value_options" target="_blank">binlog_row_value_options</a></td>
      <td><code translate="no" dir="ltr">string</code><br><code translate="no" dir="ltr">PARTIAL_JSON</code></td>
      <td>No</td>
</tr>
   <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_binlog_rows_query_log_events" target="_blank">binlog_rows_query_log_events</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">off</code>
    </td>
      <td>No</td>
  </tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_binlog_stmt_cache_size" target="_blank">binlog_stmt_cache_size</a></td>
      <td><code translate="no" dir="ltr">4096</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_binlog_transaction_dependency_history_size" target="_blank">binlog_transaction_dependency_history_size</a></td>
      <td><code translate="no" dir="ltr">integer</code>
      <p>For information about how to use this flag and its acceptable values,
      see <a href="/sql/docs/mysql/replication/manage-replicas#configuring-parallel-replication">Configuring parallel replication</a>.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_binlog_transaction_dependency_tracking" target="_blank">binlog_transaction_dependency_tracking</a></td>
      <td><code translate="no" dir="ltr">enumeration</code>
      <p>For information about how to use this flag and its acceptable values,
      see <a href="/sql/docs/mysql/replication/manage-replicas#configuring-parallel-replication">Configuring parallel replication</a>. This flag is not supported in MySQL 8.4.</p></td>
      <td>No</td>
</tr>
<tr>
     <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_block_encryption_mode" target="_blank">block_encryption_mode</a></td>
      <td><code translate="no" dir="ltr">string</code><br><code translate="no" dir="ltr">aes-keylen-mode</code>
      <br>default: <code translate="no" dir="ltr">aes-128-ECB</code>
      <br><aside class="note"><p><b>Note:</b> This flag
      takes a value in <code translate="no" dir="ltr">aes-keylen-mode</code> format, where <code translate="no" dir="ltr">keylen</code>
      is the key length (in bits) and <code translate="no" dir="ltr">mode</code> is the encryption mode.
      The value isn't case-sensitive.</p>
      <p>Allowed <code translate="no" dir="ltr">keylen</code> values are <code translate="no" dir="ltr">128</code>, <code translate="no" dir="ltr">192</code>,
      and <code translate="no" dir="ltr">256</code>.</p>
      <p>Allowed <code translate="no" dir="ltr">mode</code> values are <code translate="no" dir="ltr">ECB</code>, <code translate="no" dir="ltr">CBC</code>,
      <code translate="no" dir="ltr">CFB1</code>, <code translate="no" dir="ltr">CFB8</code>, <code translate="no" dir="ltr">CFB128</code>, and
      <code translate="no" dir="ltr">OFB</code>.</p></aside></td>
      <td>No</td>
</tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_bulk_insert_buffer_size" target="_blank">bulk_insert_buffer_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 4294967295</code>
      <br> default: <code translate="no" dir="ltr">8388608</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_collation_connection" target="_blank">collation_connection</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <br> default:
      <br>MySQL 8.0 and later - <a href="https://dev.mysql.com/doc/refman/8.0/en/charset-unicode-utf8mb4.html" target="_blank">utf8mb4_0900_ai_ci</a>
       <p>See the <a href="/sql/docs/mysql/flags#tips-option-set" target="_blank">Tips</a> section for more information about this flag.</p>
       <aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en//server-system-variables.html#sysvar_collation_server" target="_blank">collation_server</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <br>default:
      <br>MySQL 5.7 - <a href="https://dev.mysql.com/doc/refman/5.7/en/charset-unicode-utf8.html" target="_blank">utf8_general_ci</a><br>
          MySQL 8.0 and later - <a href="https://dev.mysql.com/doc/refman/8.0/en/charset-unicode-utf8mb4.html" target="_blank">utf8mb4_0900_ai_ci</a>
            <br><aside class="note"> <b>Note:</b> The <code translate="no" dir="ltr">collation_server</code> flag
      should always be set to a value that is compatible with the <code translate="no" dir="ltr">character_set_server</code>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_character_set_client" target="_blank">character_set_client</a></td>
      <td><code translate="no" dir="ltr">string</code><br>
      <br> default:
      <br>MySQL 5.7: <code translate="no" dir="ltr">utf8</code>
      <br>MySQL 8.0 and later: <code translate="no" dir="ltr">utf8mb4</code>
      <p>See the <a href="/sql/docs/mysql/flags#tips-option-set">Tips</a> section for more information about this flag.</p>
      <aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_character_set_connection" target="_blank">character_set_connection</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <br> default:
      <br>MySQL 5.7: <code translate="no" dir="ltr">utf8</code>
      <br>MySQL 8.0 and later: <code translate="no" dir="ltr">utf8mb4</code>
       <p>See the <a href="/sql/docs/mysql/flags#tips-option-set" target="_blank">Tips</a> section for more information about this flag.</p>
       <aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_character_set_results" target="_blank">character_set_results</a></td>
      <td><code translate="no" dir="ltr">string</code><br><a href="https://dev.mysql.com/doc/refman/8.0/en/charset-unicode-utf8.html" target="_blank">utf8</a> or
          <a href="https://dev.mysql.com/doc/refman/8.0/en/charset-unicode-utf8mb4.html" target="_blank">utf8mb4</a>
      <br> default:
      <br>MySQL 5.7: <code translate="no" dir="ltr">utf8</code>
      <br>MySQL 8.0 and later: <code translate="no" dir="ltr">utf8mb4</code>
       <p>See the <a href="/sql/docs/mysql/flags#tips-option-set" target="_blank">Tips</a> section for more information about this flag.</p>
       <aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_character_set_server" target="_blank">character_set_server</a></td>
      <td><code translate="no" dir="ltr">string</code><br><a href="https://dev.mysql.com/doc/refman/8.0/en/charset-unicode-utf8.html" target="_blank">utf8</a> or
          <a href="https://dev.mysql.com/doc/refman/8.0/en/charset-unicode-utf8mb4.html" target="_blank">utf8mb4</a>
          (recommended)</td>
      <td>Yes</td>
</tr>
<tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_check_proxy_users" target="_blank">check_proxy_users</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">off</code>
    </td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/analyze-table.html" target="_blank">cloudsql_allow_analyze_table</a></td>
      <td><code translate="no" dir="ltr">boolean</code> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br>default: <code translate="no" dir="ltr">off</code>
      <br><aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
    </td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-c"></a><a href="/sql/docs/mysql/manage-data-using-studio#validate-query-syntax" target="_blank">cloudsql_avoid_parse_session_logging</a></td>
      <td><code translate="no" dir="ltr">boolean</code> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br>default: <code translate="no" dir="ltr">off</code>
     </td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-c"></a><a href="/sql/docs/mysql/create-edit-iam-instances#configure-iam-db-instance" target="_blank">cloudsql_iam_authentication</a></td>
      <td><code translate="no" dir="ltr">boolean</code> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br>default: <code translate="no" dir="ltr">off</code>
      <br>Supported in MySQL 5.7 and later for Cloud SQL.
    </td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-data-encryption.html" target="_blank">cloudsql_ignore_innodb_encryption</a></td>
      <td><code translate="no" dir="ltr">boolean</code> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br>default: <code translate="no" dir="ltr">off</code>
      <br><aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
    </td>
      <td>No</td>
</tr>
    <tr>
    <td><a id="mysql-c"></a><a href="/sql/docs/mysql/use-db-audit#audit_plugin_settings">cloudsql_mysql_audit_data_masking_cmds</a></td>
    <td>
      <code translate="no" dir="ltr">string</code> <br> <code translate="no" dir="ltr">""</code>, <code translate="no" dir="ltr">dql</code>, <code translate="no" dir="ltr">dml</code>, <code translate="no" dir="ltr">ddl</code>, <code translate="no" dir="ltr">dcl</code>, <code translate="no" dir="ltr">show</code>, <code translate="no" dir="ltr">call</code>, <code translate="no" dir="ltr">create_udf</code>, <code translate="no" dir="ltr">drop_function</code>, <code translate="no" dir="ltr">create_procedure</code>, <code translate="no" dir="ltr">create_function</code>, <code translate="no" dir="ltr">drop_procedure</code>, <code translate="no" dir="ltr">alter_procedure</code>, <code translate="no" dir="ltr">alter_function</code>, <code translate="no" dir="ltr">create_trigger</code>, <code translate="no" dir="ltr">drop_trigger</code>, <code translate="no" dir="ltr">create_event</code>, <code translate="no" dir="ltr">alter_event</code>, <code translate="no" dir="ltr">drop_event</code>, <code translate="no" dir="ltr">create_db</code>, <code translate="no" dir="ltr">drop_db</code>, <code translate="no" dir="ltr">alter_db</code>, <code translate="no" dir="ltr">create_user</code>, <code translate="no" dir="ltr">drop_user</code>, <code translate="no" dir="ltr">rename_user</code>, <code translate="no" dir="ltr">alter_user</code>, <code translate="no" dir="ltr">create_table</code>, <code translate="no" dir="ltr">create_index</code>, <code translate="no" dir="ltr">alter_table</code>, <code translate="no" dir="ltr">drop_table</code>, <code translate="no" dir="ltr">drop_index</code>, <code translate="no" dir="ltr">create_view</code>, <code translate="no" dir="ltr">drop_view</code>, <code translate="no" dir="ltr">rename_table</code>, <code translate="no" dir="ltr">update</code>, <code translate="no" dir="ltr">insert</code>, <code translate="no" dir="ltr">insert_select</code>, <code translate="no" dir="ltr">delete</code>, <code translate="no" dir="ltr">truncate</code>, <code translate="no" dir="ltr">replace</code>, <code translate="no" dir="ltr">replace_select</code>, <code translate="no" dir="ltr">delete_multi</code>, <code translate="no" dir="ltr">update_multi</code>, <code translate="no" dir="ltr">load</code>, <code translate="no" dir="ltr">select</code>, <code translate="no" dir="ltr">call_procedure</code>, <code translate="no" dir="ltr">connect</code>,
<code translate="no" dir="ltr">disconnect</code>, <code translate="no" dir="ltr">grant</code>, <code translate="no" dir="ltr">revoke</code>, <code translate="no" dir="ltr">revoke_all</code>, <code translate="no" dir="ltr">show_triggers</code>, <code translate="no" dir="ltr">show_create_proc</code>, <code translate="no" dir="ltr">show_create_func</code>, <code translate="no" dir="ltr">show_procedure_code</code>, <code translate="no" dir="ltr">show_function_code</code>, <code translate="no" dir="ltr">show_create_event</code>, <code translate="no" dir="ltr">show_events</code>, <code translate="no" dir="ltr">show_create_trigger</code>, <code translate="no" dir="ltr">show_grants</code>, <code translate="no" dir="ltr">show_binlog_events</code>, <code translate="no" dir="ltr">show_relaylog_events</code>
      <br />
      <br> default: <code translate="no" dir="ltr">create_user</code>, <code translate="no" dir="ltr">alter_user</code>,
      <code translate="no" dir="ltr">grant</code>, and <code translate="no" dir="ltr">update</code>
    </td>
      <td>No</td>
  </tr>
    <tr>
    <td><a id="mysql-c"></a><a href="/sql/docs/mysql/use-db-audit#audit_plugin_settings">cloudsql_mysql_audit_data_masking_regex</a></td>
    <td>
      <code translate="no" dir="ltr">string</code> <br> <code translate="no" dir="ltr">max_string_length: 2048</code>
      <br> default: Click <a href="/sql/docs/mysql/use-db-audit#audit_plugin_settings">here</a>.
    </td>
      <td>No</td>
  </tr>
    <tr>
        <td><a id="mysql-c"></a><a href="/sql/docs/mysql/use-db-audit#audit_plugin_settings">cloudsql_mysql_audit_log_write_period</a></td>
    <td>
      <code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">0...5000</code> milliseconds
      <br> default: <code translate="no" dir="ltr">500</code> milliseconds
    </td>
      <td>No</td>
  </tr>
    <tr>
        <td><a id="mysql-c"></a><a href="/sql/docs/mysql/use-db-audit#audit_plugin_settings">cloudsql_mysql_audit_max_query_length</a></td>
    <td>
      <code translate="no" dir="ltr">integer</code> <br> <code translate="no" dir="ltr">-1...1073741824</code>
      <br> default: <code translate="no" dir="ltr">-1</code>
    </td>
      <td>No</td>
  </tr>
  <tr>
        <td><a id="mysql-c"></a><a href="/sql/docs/mysql/work-with-vectors">cloudsql_vector</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code>  <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">off</code>
      <br><aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0.36 and later for Cloud SQL.</aside>
    </td>
      <td>Yes</td>
  </tr>
  <tr>
    <td><a id="mysql-c"></a><a href="/sql/docs/mysql/search-filter-vector-embeddings#ann-iterative-filtering">cloudsql_vector_iterative_filtering</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br>
      <br> default: <code translate="no" dir="ltr">off</code>
      <br><aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0.36 and later for Cloud SQL.</aside>
    </td>
      <td>No</td>
  </tr>
    <tr>
    <td><a id="mysql-c"></a><a href="/sql/docs/mysql/search-filter-vector-embeddings#ann-iterative-filtering">cloudsql_vector_iterative_filtering_max_neighbors</a></td>
    <td>
      <code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">10...1000</code>
      <br> default: <code translate="no" dir="ltr">500</code>
      <br><aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0.36 and later for Cloud SQL.</aside>
    </td>
      <td>No</td>
  </tr>
  <tr>
        <td><a id="mysql-c"></a><a href="/sql/docs/mysql/work-with-vectors">cloudsql_vector_max_mem_size</a></td>
    <td>
      <code translate="no" dir="ltr">integer</code> <br> <code translate="no" dir="ltr">1073741824...innodb_buffer_pool_size/2</code>
      <br> default: <code translate="no" dir="ltr">1073741824</code> in bytes
       <br><aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0.36 and later for Cloud SQL.</aside>
    </td>
      <td>Yes</td>
  </tr>
    <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_completion_type" target="_blank">completion_type</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">NO_CHAIN</code> (default),
      <code translate="no" dir="ltr">CHAIN</code>, or <code translate="no" dir="ltr">RELEASE</code></td>
      <td>No</td>
</tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_concurrent_insert" target="_blank">concurrent_insert</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">NEVER</code>, <code translate="no" dir="ltr">AUTO</code>
       (default), or <code translate="no" dir="ltr">ALWAYS</code></td>
      <td>No</td>
</tr>
    <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_connect_timeout" target="_blank">connect_timeout</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">2</code> ... <code translate="no" dir="ltr">31536000</code>
      <br> default: <code translate="no" dir="ltr">10</code></td>
      <td>No</td>
</tr>
    <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_connection_memory_chunk_size" target="_blank">connection_memory_chunk_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">536870912</code>
      <br>Default: <br>
            MySQL 8.0.28 to 8.0.33: <code translate="no" dir="ltr">8912</code><br>
            MySQL 8.0.34 and later: <code translate="no" dir="ltr">8192</code>
      <br><aside class="note"> <b>Note</b>: The flag is supported in MySQL 8.0.28 and later
        for Cloud SQL.</aside></td>
      <td>No</td>
</tr>
    <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_connection_memory_limit" target="_blank">connection_memory_limit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">2097152</code> ... <code translate="no" dir="ltr">9223372036854775807</code>
      <br><aside class="note"> <b>Note</b>: The flag is supported in MySQL 8.0.28 and later
        for Cloud SQL.</aside></td>
      <td>No</td>
</tr>
    <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_cte_max_recursion_depth" target="_blank">cte_max_recursion_depth</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">4294967295</code>
      <br> default: <code translate="no" dir="ltr">1000</code></td>
      <td>No</td>
</tr>
  <tr>
      <td><a id="mysql-d"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_default_authentication_plugin" target="_blank">default_authentication_plugin</a></td>
      <td><code translate="no" dir="ltr">string</code> <br> <code translate="no" dir="ltr">mysql_native_password|caching_sha2_password</code>
    <br><aside class="note"> <b>Note</b>: This flag is supported in MySQL 8.0
        for Cloud SQL only. The flag is not supported in MySQL 8.4.</aside>
    </td>
      <td>Yes</td>
  </tr>
  <tr>
      <td><a id="mysql-d"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_default_password_lifetime" target="_blank">default_password_lifetime</a></td>
      <td><code translate="no" dir="ltr">integer</code> <code translate="no" dir="ltr">0...65535</code>
        <br> default: <code translate="no" dir="ltr">0</code>
    <br><aside class="note"> <b>Note</b>: The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
    </td>
      <td>No</td>
  </tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-options.html#option_mysqld_default-time-zone" target="_blank">default_time_zone</a></td>
      <td><code translate="no" dir="ltr">string</code><br>There are two ways to specify timezones: as timezone
        offsets and timezone names. For example, <code translate="no" dir="ltr">+00:00</code> is the
        timezone offset for London (which is in the UTC timezone), and
        <code translate="no" dir="ltr">Europe/London</code> is its timezone name.<p>
        <p>You use values to specify timezone offsets, from <code translate="no" dir="ltr">-12:59</code> to <code translate="no" dir="ltr">+13:00</code>. Leading zeros are required.</p>
        <p>When using timezone names, automatic adjustment to daylight saving time is supported. When using timezone offsets, it isn't supported. See a list of <a href="#timezone-names">timezone names</a> that Cloud SQL for MySQL supports. You must update this flag manually, on the primary instance and on all read replicas, to account for it.</p>
        <p>To set the timezone without causing a restart of the Cloud SQL instance, use the <code translate="no" dir="ltr">set time_zone=<var translate="no">timezone_offset</var></code> or <code translate="no" dir="ltr"><var translate="no">timezone_name</var></code> command with the <code translate="no" dir="ltr">init_connect</code> flag.</p>
  </td>
  <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_default_tmp_storage_engine" target="_blank">default_tmp_storage_engine</a></td>
      <td><code translate="no" dir="ltr">string</code> <br> <code translate="no" dir="ltr">INNODB|MEMORY</code>
      <br> default: <code translate="no" dir="ltr">INNODB</code>
      </td>
      <td>No</td>
</tr>
    <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_default_week_format" target="_blank">default_week_format</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">7</code>
      <br> default: <code translate="no" dir="ltr">0</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_delay_key_write" target="_blank">delay_key_write</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">OFF</code>, <code translate="no" dir="ltr">ON</code> (default),
      or <code translate="no" dir="ltr">ALL</code></td>
      <td>No</td>
</tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_disconnect_on_expired_password" target="_blank">disconnect_on_expired_password </a></td>
      <td><code translate="no" dir="ltr">boolean</code> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        <br> default: <code translate="no" dir="ltr">on</code>
    <br><aside class="note"> <b>Note</b>: The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
    </td>
      <td>Yes</td>
  </tr>
      <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_div_precision_increment" target="_blank">div_precision_increment</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">30</code>
      <br> default: <code translate="no" dir="ltr">4</code></td>
      <td>No</td>
</tr>
   <tr>
    <td><a id="mysql-e"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_end_markers_in_json" target="_blank">end_markers_in_json</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">off</code>
    </td>
      <td>No</td>
  </tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_eq_range_index_dive_limit" target="_blank">eq_range_index_dive_limit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 2147483647</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_event_scheduler" target="_blank">event_scheduler</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        <p>If you are using the Event Scheduler, configure your
        instance with an
        <a href="/sql/docs/mysql/start-stop-restart-instance#activation_policy" target="_blank">
        activation policy</a> of ALWAYS to ensure that scheduled events run.</p>
        <p>See the
          <a href="/sql/docs/mysql/flags#tips-event-scheduler" target="_blank">Tips</a>
          section for more information about this flag.</p>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_expire_logs_days" target="_blank">expire_logs_days</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">99</code> <br />Default is 0, which means no automatic removal.
      <p><b>Note</b>: This flag is not supported in MySQL 8.4.
      Use <code translate="no" dir="ltr">binlog_expire_logs_seconds</code> instead. See the
        <a href="/sql/docs/mysql/flags#tips-expire-logs" target="_blank">Tips</a>
        section for more information about this flag.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_explicit_defaults_for_timestamp" target="_blank">explicit_defaults_for_timestamp</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code><br><br>
      <aside class="note"> <b>Note:</b> The flag only requires a restart for MySQL 5.6.x or earlier.</aside></td>
      <td>No</td>
</tr>
      <tr>
      <td><a id="mysql-f"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_flush_time" target="_blank">flush_time</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">31536000</code>
      <br> default: <code translate="no" dir="ltr">0</code></td>
      <td>No</td>
</tr>
   <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_foreign_key_checks" target="_blank">foreign_key_checks</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">on</code>
     <p>See the <a href="/sql/docs/mysql/flags#tips-option-set">Tips</a> section for more information about this flag.</p>
     <aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
     </td>
      <td>No</td>
  </tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_ft_max_word_len" target="_blank">ft_max_word_len</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">10</code> ... <code translate="no" dir="ltr"> 252</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_ft_min_word_len" target="_blank">ft_min_word_len</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 16</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_ft_query_expansion_limit" target="_blank">ft_query_expansion_limit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1000</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_ft_stopword_file" target="_blank">ft_stopword_file</a></td>
      <td><code translate="no" dir="ltr">string</code><br></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a id="mysql-g"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_general_log" target="_blank">general_log</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <p>See the <a href="/sql/docs/mysql/flags#tips-general-log" target="_blank">Tips</a> section for more information about general logs.</p></td>
      <td>No</td>
</tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_generated_random_password_length" target="_blank">generated_random_password_length</a></td>
      <td><code translate="no" dir="ltr">integer</code> <code translate="no" dir="ltr"> 5-255</code>
        <br> default: <code translate="no" dir="ltr">20</code>
    <br><aside class="note"> <b>Note</b>: The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
    </td>
      <td>No</td>
</tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_global_connection_memory_limit" target="_blank">global_connection_memory_limit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">16777216</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code>
      <br><aside class="note"> <b>Note</b>: The flag is supported in MySQL 8.0.28 and later
        for Cloud SQL.</aside>
      </td>
      <td>No</td>
</tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_global_connection_memory_tracking" target="_blank">global_connection_memory_tracking</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> Default: <code translate="no" dir="ltr">off</code>
      <br><aside class="note"> <b>Note</b>: The flag is supported in MySQL 8.0.28 and later
        for Cloud SQL.</aside>
      </td>
      <td>No</td>
</tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_group_concat_max_len" target="_blank">group_concat_max_len</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">4</code> ... <code translate="no" dir="ltr"> 17179869184</code></td>
      <td>No</td>
</tr>
      <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-gtids.html#sysvar_gtid_executed_compression_period" target="_blank">gtid_executed_compression_period</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">4294967295</code>
      <br> default (up to version 8.0.22): <code translate="no" dir="ltr">1000</code>
      <br> default (version 8.0.23+): <code translate="no" dir="ltr">0</code>
      </td>
      <td>No</td>
</tr>
      <tr>
      <td><a id="mysql-h"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_histogram_generation_max_mem_size" target="_blank">histogram_generation_max_mem_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1000000</code> ... <code translate="no" dir="ltr">4294967295</code>
      <br> default: <code translate="no" dir="ltr">20000000</code>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-i"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_init_connect" target="_blank">init_connect</a></td>
      <td><code translate="no" dir="ltr">string</code></td>
      <td>No</td>
</tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_adaptive_hash_index" target="_blank">innodb_adaptive_hash_index</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_adaptive_hash_index_parts" target="_blank">innodb_adaptive_hash_index_parts</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 512</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_adaptive_max_sleep_delay" target="_blank">innodb_adaptive_max_sleep_delay</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1000000</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_autoextend_increment" target="_blank">innodb_autoextend_increment</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">1000</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_autoinc_lock_mode" target="_blank">innodb_autoinc_lock_mode</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 2</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_buffer_pool_chunk_size" target="_blank">innodb_buffer_pool_chunk_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1048576</code> ... <code translate="no" dir="ltr">(innodb_buffer_pool_size/innodb_buffer_pool_instances)</code><br>
      <p>This flag value is dependent on <code translate="no" dir="ltr">innodb_buffer_pool_size</code>
      and <code translate="no" dir="ltr">innodb_buffer_pool_instances</code>. MySQL can auto-tune the
      value of <code translate="no" dir="ltr">innodb_buffer_pool_chunk_size</code> based on these two
      flags. </p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_buffer_pool_dump_pct" target="_blank">innodb_buffer_pool_dump_pct</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 100</code>
        <br>Default: <code translate="no" dir="ltr">25</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_buffer_pool_dump_at_shutdown" target="_blank">innodb_buffer_pool_dump_at_shutdown</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_buffer_pool_dump_now" target="_blank">innodb_buffer_pool_dump_now</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
       <p>See the <a href="/sql/docs/mysql/flags#tips-option-set">Tips</a>
       section for more information about this flag.</p>
       <aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_buffer_pool_instances" target="_blank">innodb_buffer_pool_instances</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 64</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_buffer_pool_load_abort" target="_blank">
      innodb_buffer_pool_load_abort</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
       <p>See the <a href="/sql/docs/mysql/flags#tips-option-set">Tips</a>
       section for more information about this flag.</p>
       <aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_buffer_pool_load_at_startup" target="_blank">innodb_buffer_pool_load_at_startup</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_buffer_pool_load_now" target="_blank">innodb_buffer_pool_load_now</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
       <p>See the <a href="/sql/docs/mysql/flags#tips-option-set">Tips</a>
       section for more information about this flag.</p>
       <aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-buffer-pool-size.html" target="_blank">innodb_buffer_pool_size</a></td>
      <td><code translate="no" dir="ltr">integer</code>
        <p>Setting this flag for MySQL 5.6 requires a restart. See the
         <a href="/sql/docs/mysql/flags#tips-buffer-pool">Tips</a> section for
         more information about this flag.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_change_buffer_max_size" target="_blank">innodb_change_buffer_max_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 50</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_change_buffering" target="_blank">innodb_change_buffering</a>
      </td>
      <td><code translate="no" dir="ltr">string</code><br><p>Options: <code translate="no" dir="ltr">none</code>,
        <code translate="no" dir="ltr">inserts</code>, <code translate="no" dir="ltr">deletes</code>, <code translate="no" dir="ltr">changes</code>,
        <code translate="no" dir="ltr">purges</code>, <code translate="no" dir="ltr">all</code>.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_checksum_algorithm" target="_blank">innodb_checksum_algorithm</a>
      </td>
      <td><code translate="no" dir="ltr">string</code><br><p>Options: <code translate="no" dir="ltr">crc32</code>,
        <code translate="no" dir="ltr">strict_crc32</code>, <code translate="no" dir="ltr">innodb</code>,
        <code translate="no" dir="ltr">strict_innod</code>, <code translate="no" dir="ltr">none</code>,  <code translate="no" dir="ltr">strict_none</code>.
        </p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="/sql/docs/mysql/optimize-high-memory-usage#enable-managed-buffer-pool">innodb_cloudsql_managed_buffer_pool</a>
      </td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
       <br>Default: <code translate="no" dir="ltr">off</code>
       <br>
       (<a href="https://cloud.google.com/products#product-launch-stages">Preview</a>)</td>
      <td>No</td>
</tr>
<tr>
      <td><a href="/sql/docs/mysql/optimize-high-memory-usage#enable-managed-buffer-pool">innodb_cloudsql_managed_buffer_pool_threshold_pct</a>
      </td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">50</code> ... <code translate="no" dir="ltr">99</code>
       <br>Default: <code translate="no" dir="ltr">95</code>
       <br>
       (<a href="https://cloud.google.com/products#product-launch-stages">Preview</a>)</td>
      <td>No</td>
</tr>
<tr>
      <td><a href="#tips-optimized-write">innodb_cloudsql_optimized_write</a>
      </td>
      <td><code translate="no" dir="ltr">boolean</code>
       <p>This flag is available only for instances with Cloud SQL Enterprise Plus edition.
        For more information about this flag, see the
      <a href="/sql/docs/mysql/flags#tips-optimized-write">Tips</a> section.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_cmp_per_index_enabled" target="_blank">innodb_cmp_per_index_enabled</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_commit_concurrency" target="_blank">innodb_commit_concurrency</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">1000</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_compression_failure_threshold_pct" target="_blank">innodb_compression_failure_threshold_pct</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">100</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_compression_level" target="_blank">innodb_compression_level</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">9</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_compression_pad_pct_max" target="_blank">innodb_compression_pad_pct_max</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">75</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_concurrency_tickets" target="_blank">innodb_concurrency_tickets</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 4294967295</code></td>
      <td>No</td>
</tr>

<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_deadlock_detect" target="_blank">innodb_deadlock_detect</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <p>Supported in MySQL <a href="https://dev.mysql.com/doc/refman/5.7/en/innodb-parameters.html#sysvar_innodb_deadlock_detect">5.7</a> and later.</p>
      <p>Default: <code translate="no" dir="ltr">on</code></p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_disable_sort_file_cache" target="_blank">innodb_disable_sort_file_cache</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_doublewrite_batch_size" target="_blank">innodb_doublewrite_batch_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 256</code><br>
      Default: <code translate="no" dir="ltr">0</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_doublewrite_files" target="_blank">innodb_doublewrite_files</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">2</code> ... <code translate="no" dir="ltr"> 128</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_doublewrite_pages" target="_blank">innodb_doublewrite_pages</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">4</code> ... <code translate="no" dir="ltr"> 512</code><br>
      Default: <code translate="no" dir="ltr">64</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_file_per_table" target="_blank">innodb_file_per_table</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <p>See the <a href="/sql/docs/mysql/flags#tips-file-per-table">Tips</a> section for more information about this flag.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_fill_factor" target="_blank">innodb_fill_factor</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">10</code> ... <code translate="no" dir="ltr"> 100</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_flush_log_at_timeout" target="_blank">innodb_flush_log_at_timeout</a></td>
      <td><code translate="no" dir="ltr">double</code><br><code translate="no" dir="ltr">0.0001</code>... <code translate="no" dir="ltr">2700</code><br />
      Default: <code translate="no" dir="ltr">1</code>
      <p>Supported in MySQL <a href="https://dev.mysql.com/doc/refman/5.7/en/innodb-parameters.html#sysvar_innodb_flush_log_at_timeout">5.7</a>
      and later.</p>
      <p>See the <a href="/sql/docs/mysql/flags#tips-flush-log-timeout">Tips</a> section for more information about this flag.</p>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_flush_log_at_trx_commit" target="_blank">innodb_flush_log_at_trx_commit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1, 2</code><br />
      Default: <code translate="no" dir="ltr">1</code>
      <p>If you promote a replica with this flag enabled,
        the flag is automatically removed causing the promoted replica to have full durability
        by default. To use this flag with a promoted replica, you can update the flag to
        the replica after promotion.</p>
      <aside class="note"> <b>Note: </b>If you change the default value for the <code translate="no" dir="ltr">innodb_flush_log_at_trx_commit</code>
      flag on an HA-enabled instance, then the instance loses SLA coverage because durability might decrease.</aside>
      <p>See the <a href="/sql/docs/mysql/flags#tips-flush-log">Tips</a> section for more information about this flag.</p></td>
      <td>No</td>
</tr>

<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_flush_method" target="_blank">innodb_flush_method</a></td>
      <td><code translate="no" dir="ltr">string</code><p>Options: <code translate="no" dir="ltr">fsync</code>,
      <code translate="no" dir="ltr">O_DIRECT</code>, <code translate="no" dir="ltr">O_DSYNC</code></p>
      Default: <code translate="no" dir="ltr">O_DIRECT</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_flush_neighbors" target="_blank">innodb_flush_neighbors</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">2</code><br/>
       Default values:<br>
      <li>MySQL 5.6:<code translate="no" dir="ltr"> 0</code></li>
      <li>MySQL 5.7 and later:<code translate="no" dir="ltr"> 2</code></li>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_flush_sync" target="_blank">innodb_flush_sync</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_flushing_avg_loops" target="_blank">innodb_flushing_avg_loops</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 1000</code><br>
      Default: <code translate="no" dir="ltr">30</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_fsync_threshold" target="_blank">innodb_fsync_threshold</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code><br>
      Default: <code translate="no" dir="ltr">0</code>
      <aside class="note"> <b>Note</b>: The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_aux_table" target="_blank">innodb_ft_aux_table</a></td>
      <td><code translate="no" dir="ltr">string</code><br>
       <p>See the <a href="/sql/docs/mysql/flags#tips-option-set" target="_blank">Tips</a> section for more information about this flag.</p>
       <aside class="note"> <b>Note</b>: The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_cache_size" target="_blank">innodb_ft_cache_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1600000</code> ... <code translate="no" dir="ltr"> 80000000</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_enable_diag_print" target="_blank">innodb_ft_enable_diag_print</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_enable_stopword" target="_blank">innodb_ft_enable_stopword</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_max_token_size" target="_blank">innodb_ft_max_token_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">10</code> ... <code translate="no" dir="ltr"> 252</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_min_token_size" target="_blank">innodb_ft_min_token_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 16</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_num_word_optimize" target="_blank">innodb_ft_num_word_optimize</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1000</code> ... <code translate="no" dir="ltr"> 10000</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_result_cache_limit" target="_blank">innodb_ft_result_cache_limit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1000000</code> ... <code translate="no" dir="ltr"> 4294967295</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_server_stopword_table" target="_blank">innodb_ft_server_stopword_table</a></td>
      <td><code translate="no" dir="ltr">string</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_sort_pll_degree" target="_blank">innodb_ft_sort_pll_degree</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">32</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_total_cache_size" target="_blank">innodb_ft_total_cache_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">32000000</code> ... <code translate="no" dir="ltr"> 1600000000</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_ft_user_stopword_table" target="_blank">innodb_ft_user_stopword_table</a></td>
      <td><code translate="no" dir="ltr">string</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_idle_flush_pct" target="_blank">innodb_idle_flush_pct</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 100</code>
        <br>Default: <code translate="no" dir="ltr">100</code>
        <aside class="note"> <b>Note</b>: The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_io_capacity" target="_blank">innodb_io_capacity</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">100</code> ... <code translate="no" dir="ltr"> 100000</code>
        <br>Default: <code translate="no" dir="ltr">5000</code>
        <p> To learn more about configuring the disk performance, see the <strong>E2 VMs</strong> table in
        <a href="/compute/docs/disks/performance#e2_vms" target="_blank">Configure disks to meet performance requirements.</a> </p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_io_capacity_max" target="_blank">innodb_io_capacity_max</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">100</code> ... <code translate="no" dir="ltr"> 100000</code>
        <br>Default: <code translate="no" dir="ltr">10000</code>
        <p>To learn more about configuring the disk performance, see the <strong>E2 VMs</strong> table in
        <a href="/compute/docs/disks/performance#e2_vms" target="_blank">Configure disks to meet performance requirements.</a></p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_large_prefix" target="_blank">innodb_large_prefix</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        <p>Supported only in MySQL 5.6.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_lock_wait_timeout" target="_blank">innodb_lock_wait_timeout</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">1073741824</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_log_buffer_size" target="_blank">innodb_log_buffer_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">262144</code> ... <code translate="no" dir="ltr"> 4294967295</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_log_checksums" target="_blank">innodb_log_checksums</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        <br>default: <code translate="no" dir="ltr">on</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_log_file_size" target="_blank">innodb_log_file_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br>
        MySQL 5.6: <code translate="no" dir="ltr">1048576</code> ... <code translate="no" dir="ltr">274877906944</code><br>
        MySQL 5.7 and later: <code translate="no" dir="ltr">4194304</code> ... <code translate="no" dir="ltr">274877906944</code>
        </td>
      <td>Yes</td>
      <p>For more information about this flag, see the <a href="/sql/docs/mysql/flags#tips-redo-logs">Tips</a> section.</p>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_log_spin_cpu_abs_lwm" target="_blank">innodb_log_spin_cpu_abs_lwm</a></td>
      <td><code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">4294967295</code>
        <br>default: <code translate="no" dir="ltr">80</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_log_spin_cpu_pct_hwm" target="_blank">innodb_log_spin_cpu_pct_hwm</a></td>
      <td><code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">100</code>
        <br>default: <code translate="no" dir="ltr">50</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_log_wait_for_flush_spin_hwm" target="_blank">innodb_log_wait_for_flush_spin_hwm</a></td>
      <td><code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">4294967295</code>
         <br>default: <code translate="no" dir="ltr">400</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_log_write_ahead_size" target="_blank">innodb_log_write_ahead_size</a></td>
      <td><code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">512</code> ... <code translate="no" dir="ltr">65536</code>
         <br>default: <code translate="no" dir="ltr">8192</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_lru_scan_depth" target="_blank">innodb_lru_scan_depth</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">100</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>


<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_max_dirty_pages_pct" target="_blank">innodb_max_dirty_pages_pct</a></td>
      <td><code translate="no" dir="ltr">float</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">99.99</code>
         <br>default: <code translate="no" dir="ltr">90</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_max_dirty_pages_pct_lwm" target="_blank">innodb_max_dirty_pages_pct_lwm</a></td>
      <td><code translate="no" dir="ltr">float</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">99.99</code>
         <br>default: <code translate="no" dir="ltr">10</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_max_purge_lag" target="_blank">innodb_max_purge_lag</a></td>
      <td><code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">4294967295</code>
        <br>default: <code translate="no" dir="ltr">0</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_max_undo_log_size" target="_blank">innodb_max_undo_log_size</a></td>
      <td><code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">10485760</code> ... <code translate="no" dir="ltr">9223372036854775807</code>
        <br>default: <code translate="no" dir="ltr">1073741824</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_max_purge_lag_delay" target="_blank">innodb_max_purge_lag_delay</a></td>
      <td><code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">10000000</code>
        <br>default: <code translate="no" dir="ltr">0</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_monitor_disable" target="_blank">innodb_monitor_disable</a></td>
    <td>
      <code translate="no" dir="ltr">string</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_monitor_enable" target="_blank">innodb_monitor_enable</a></td>
    <td>
      <code translate="no" dir="ltr">string</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_monitor_reset" target="_blank">innodb_monitor_reset</a></td>
    <td>
      <code translate="no" dir="ltr">string</code> <br> <code translate="no" dir="ltr">counter</code>, <code translate="no" dir="ltr">module</code>, <code translate="no" dir="ltr">pattern</code>, <code translate="no" dir="ltr">all</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_monitor_reset_all" target="_blank">innodb_monitor_reset_all</a></td>
    <td>
      <code translate="no" dir="ltr">enumeration</code> <br> Valid values: <code translate="no" dir="ltr">counter</code>, <code translate="no" dir="ltr">module</code>, <code translate="no" dir="ltr">pattern</code>, <code translate="no" dir="ltr">all</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_old_blocks_pct" target="_blank">innodb_old_blocks_pct</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">5</code> ... <code translate="no" dir="ltr"> 95</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_old_blocks_time" target="_blank">innodb_old_blocks_time</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 4294967295</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_online_alter_log_max_size" target="_blank">innodb_online_alter_log_max_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">65536</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_open_files" target="_blank">innodb_open_files</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">100</code> ... <code translate="no" dir="ltr">2147483647</code>
          <br>default: <br> MySQL 5.7: <code translate="no" dir="ltr">2000</code>
          <br> MySQL 8.0 and later: <code translate="no" dir="ltr">4000</code> </td>
      <td>&ge; <code translate="no" dir="ltr">8.0.28</code>: No <br> &le; <code translate="no" dir="ltr">8.0.27</code>: Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_optimize_fulltext_only" target="_blank">innodb_optimize_fulltext_only</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>

<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_page_cleaners" target="_blank">innodb_page_cleaners</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 64</code><br>
      Supported in MySQL <a href="https://dev.mysql.com/doc/refman/5.7/en/innodb-parameters.html#sysvar_innodb_page_cleaners">5.7</a> and later.
      For MySQL 5.7 and 8.0, the default is <code translate="no" dir="ltr">4</code>.
      For MySQL 8.4, the default is equal to the value
      of configured for the <code translate="no" dir="ltr"><a href="https://dev.mysql.com/doc/refman/8.4/en/innodb-parameters.html#sysvar_innodb_page_cleaners">innodb_buffer_pool_instances</a></code> flag.</td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_parallel_read_threads" target="_blank">innodb_parallel_read_threads</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">256</code>
          <br>default: <code translate="no" dir="ltr">4</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_print_all_deadlocks" target="_blank">innodb_print_all_deadlocks</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
          <br>default: <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_print_ddl_logs" target="_blank">innodb_print_ddl_logs</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_purge_batch_size" target="_blank">innodb_purge_batch_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">5000</code>
          <br>default: <code translate="no" dir="ltr">300</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_purge_rseg_truncate_frequency" target="_blank">innodb_purge_rseg_truncate_frequency</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">128</code>
          <br>default: <code translate="no" dir="ltr">128</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_purge_threads" target="_blank">innodb_purge_threads</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">32</code>
      <br>Default values:<br>
      <li>MySQL 5.6:<code translate="no" dir="ltr"> 1</code></li>
      <li>MySQL 5.7 and later:<code translate="no" dir="ltr"> 4</code></li>
      </td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_random_read_ahead" target="_blank">innodb_random_read_ahead</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_read_ahead_threshold" target="_blank">innodb_read_ahead_threshold</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 64</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_read_io_threads" target="_blank">innodb_read_io_threads</a></td>
      <td><code translate="no" dir="ltr">integer</code><br>
        <code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">64</code>
        </td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_redo_log_capacity" target="_blank">innodb_redo_log_capacity</a></td>
      <td><code translate="no" dir="ltr">integer</code><br>
        MySQL 8.0.33 and earlier: <code translate="no" dir="ltr">8388608</code> (8&nbsp;MB) ... <code translate="no" dir="ltr">137438953472</code> (128&nbsp;GB)
        MySQL 8.0.34 and later: <code translate="no" dir="ltr">8388608</code> (8&nbsp;MB) ... <code translate="no" dir="ltr">549755813888</code> (512&nbsp;GB)
        <p>For more information about this flag, see the <a href="/sql/docs/mysql/flags#tips-redo-logs">Tips</a> section.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_replication_delay" target="_blank">innodb_replication_delay</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 4294967295</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_rollback_on_timeout" target="_blank">innodb_rollback_on_timeout</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_rollback_segments" target="_blank">innodb_rollback_segments</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 128</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_segment_reserve_factor" target="_blank">innodb_segment_reserve_factor</a></td>
      <td><code translate="no" dir="ltr">float</code><br><code translate="no" dir="ltr">.03</code> ... <code translate="no" dir="ltr"> 40</code>
      <br>default: <code translate="no" dir="ltr">12.5</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_sort_buffer_size" target="_blank">innodb_sort_buffer_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">65536</code> ... <code translate="no" dir="ltr"> 67108864</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_spin_wait_delay" target="_blank">innodb_spin_wait_delay</a></td>
      <td><code translate="no" dir="ltr">integer</code><br>
            MySQL 5.7: <code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1000000</code><br>
            MySQL 8.0.13+: <code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1000</code>
      <br>default: <code translate="no" dir="ltr">6</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_stats_auto_recalc" target="_blank">innodb_stats_auto_recalc</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_stats_include_delete_marked" target="_blank">innodb_stats_include_delete_marked</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br>default: <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_stats_method" target="_blank">innodb_stats_method</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">nulls_equal</code> | <code translate="no" dir="ltr">nulls_unequal</code> | <code translate="no" dir="ltr">nulls_ignored</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_stats_on_metadata" target="_blank">innodb_stats_on_metadata</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_stats_persistent" target="_blank">innodb_stats_persistent</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_stats_persistent_sample_pages" target="_blank">innodb_stats_persistent_sample_pages</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_stats_sample_pages" target="_blank">innodb_stats_sample_pages</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_stats_transient_sample_pages" target="_blank">innodb_stats_transient_sample_pages</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_status_output" target="_blank">innodb_status_output</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_status_output_locks" target="_blank">innodb_status_output_locks</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_strict_mode" target="_blank">innodb_strict_mode</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_sync_array_size" target="_blank">innodb_sync_array_size</a></td>
      <td><code translate="no" dir="ltr">1 ... 1024</code>
  <p>Default is <b>1</b>.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_sync_spin_loops" target="_blank">innodb_sync_spin_loops</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 4294967295</code>
      <br>default: </code>30</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_table_locks" target="_blank">innodb_table_locks</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br>default: <code translate="no" dir="ltr">on</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_thread_concurrency" target="_blank">innodb_thread_concurrency</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1000</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_thread_sleep_delay" target="_blank">innodb_thread_sleep_delay</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1000000</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_undo_log_truncate" target="_blank">innodb_undo_log_truncate</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br>default: <code translate="no" dir="ltr">on</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_use_native_aio" target="_blank">innodb_use_native_aio</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br>default: <code translate="no" dir="ltr">on</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_write_io_threads" target="_blank">innodb_write_io_threads</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 64</code></td>
      <td>Yes</td>
</tr>

<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_interactive_timeout" target="_blank">interactive_timeout</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 31536000</code></td>
      <td>No</td>
</tr>

<tr>
      <td><a href="https://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html#sysvar_internal_tmp_disk_storage_engine" target="_blank">internal_tmp_disk_storage_engine</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">INNODB</code> | <code translate="no" dir="ltr">MYISAM</code><br>
      Default: <code translate="no" dir="ltr">INNODB</code><br>
     <aside class="note"> <b>Note</b>: This flag is supported in MySQL 5.7
     for Cloud SQL only.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_internal_tmp_mem_storage_engine" target="_blank">internal_tmp_mem_storage_engine</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">MEMORY</code>, <code translate="no" dir="ltr">TempTable</code><br>
      <aside class="note"> <b>Note</b>: The flag is supported in MySQL 8.0 and later
      for Cloud SQL.</aside>.</td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-j"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_join_buffer_size" target="_blank">join_buffer_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">128</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_keep_files_on_create" target="_blank">keep_files_on_create</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">off</code>
    </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_key_buffer_size" target="_blank">key_buffer_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">4096</code> ... <code translate="no" dir="ltr"> 4294967295</code>
      <br>default: <code translate="no" dir="ltr">8388608</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_key_cache_age_threshold" target="_blank">key_cache_age_threshold</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">100</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code>
      <br>default: <code translate="no" dir="ltr">300</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_key_cache_block_size" target="_blank">key_cache_block_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">512</code> ... <code translate="no" dir="ltr"> 16384</code>
      <br>default: <code translate="no" dir="ltr">1024</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_key_cache_division_limit" target="_blank">key_cache_division_limit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 100</code>
      <br>default: <code translate="no" dir="ltr">100</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_lc_time_names" target="_blank">lc_times_names</a></td>
      <td><code translate="no" dir="ltr">string</code><br><code translate="no" dir="ltr">en_US</code> | <code translate="no" dir="ltr">cs_CZ</code> | <code translate="no" dir="ltr">da_DK</code> | <code translate="no" dir="ltr">nl_NL</code> | <code translate="no" dir="ltr">et_EE</code> | <code translate="no" dir="ltr">fr_FR</code> | <code translate="no" dir="ltr">de_DE</code> | <code translate="no" dir="ltr">el_GR</code> | <code translate="no" dir="ltr">hu_HU</code> | <code translate="no" dir="ltr">it_IT</code> | <code translate="no" dir="ltr">ja_JP</code> | <code translate="no" dir="ltr">ko_KR</code> | <code translate="no" dir="ltr">no_NO</code> | <code translate="no" dir="ltr">nb_NO</code> | <code translate="no" dir="ltr">pl_PL</code> | <code translate="no" dir="ltr">pt_PT</code> | <code translate="no" dir="ltr">ro_RO</code> | <code translate="no" dir="ltr">ru_RU</code> | <code translate="no" dir="ltr">sr_RS</code> | <code translate="no" dir="ltr">sk_SK</code> | <code translate="no" dir="ltr">es_ES</code> | <code translate="no" dir="ltr">sv_SE</code> | <code translate="no" dir="ltr">uk_UA</code>
      <br>default: <code translate="no" dir="ltr">en_US</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-l"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_local_infile" target="_blank">local_infile</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_lock_wait_timeout" target="_blank">lock_wait_timeout</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 31536000</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_log_bin_trust_function_creators" target="_blank">log_bin_trust_function_creators</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_log_output" target="_blank">log_output</a></td>
      <td><code translate="no" dir="ltr">set</code><br><code translate="no" dir="ltr">FILE</code> | <code translate="no" dir="ltr">TABLE</code> | <code translate="no" dir="ltr">NONE</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_log_error_verbosity" target="_blank">log_error_verbosity</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 3</code>
      <br>default: <br>
            MySQL 5.7: <code translate="no" dir="ltr">3</code><br>
            MySQL 8.0 and later: <code translate="no" dir="ltr">2</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_log_queries_not_using_indexes" target="_blank">log_queries_not_using_indexes</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_log_slow_admin_statements" target="_blank">log_slow_admin_statements</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br>default: <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_log_slow_extra" target="_blank">log_slow_extra</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br>default: <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_log_slow_replica_statements" target="_blank">log_slow_replica_statements</a></td>
      <td><code translate="no" dir="ltr">boolean</code>
        <br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        Default: <code translate="no" dir="ltr">off</code>
         <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use the <code translate="no" dir="ltr">log_slow_slave_statements</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
      </td>
        <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_log_slow_slave_statements" target="_blank">log_slow_slave_statements</a></td>
      <td><code translate="no" dir="ltr">boolean</code>
        <br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        Default: <code translate="no" dir="ltr">off</code>
        <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">log_slow_replica_statements</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
      </td>
        <td>No</td>
</tr>
<tr>
          <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_log_throttle_queries_not_using_indexes" target="_blank">log_throttle_queries_not_using_indexes</a></td>
          <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
          <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_log_timestamps" target="_blank">log_timestamps</a></td>
      <td><code translate="no" dir="ltr">string</code><br><code translate="no" dir="ltr">"UTC | SYSTEM"</code>
      <br>default: <code translate="no" dir="ltr">UTC</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_long_query_time" target="_blank">long_query_time</a></td>
      <td><code translate="no" dir="ltr">float</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">30000000</code>
        <p>Cloud SQL provides the ability to set this flag to less than 1 if needed.</p>
        <p>If the <code translate="no" dir="ltr">log_queries_not_using_indexes</code> flag is also
           enabled, you may see queries with less than the time specified here.</p></td>
      <td>No</td>
</tr>
<tr>
  <td>lower_case_table_names <a href="https://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html#sysvar_lower_case_table_names" target="_blank">5.7</a> | <a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_lower_case_table_names" target="_blank">8.0</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> or <code translate="no" dir="ltr">1</code>
        <br>Default: <code translate="no" dir="ltr">0</code>
        <p>If you use the default value of <code translate="no" dir="ltr">0</code> for this flag,
          table and database names are
          case sensitive. When set to <code translate="no" dir="ltr">1</code>,
          table and database names are case insensitive.</p>
      <p>For MySQL 5.7 instances, you can change the value of this flag at any
        time. If you do, then make sure that you understand how the change affects your
        existing tables and databases.</p>
        <p><b>For MySQL 8.0 and later instances, you can set the value of this flag
          to a desired value only while an instance is being created.</b> After you set this value,
          you can't change it. Also, for an existing instance, you can't change the value of this flag.
        </p>
        <p>When creating read replicas for MySQL 5.7, MySQL 8.0, or MySQL 8.4 instances,
          the replica inherits this flag value from the primary.
        </p>
      <td>Yes</td>
</tr>
  <tr>
      <td><a id="mysql-m"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_mandatory_roles" target="_blank">mandatory_roles</a></td>
      <td><code translate="no" dir="ltr">string</code> <code translate="no" dir="ltr"><i>role name</i></code>
        <br> default: <code translate="no" dir="ltr">empty string</code>
    <br><aside class="note"> <b>Note:  </b>The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
    </td>
      <td>No</td>
  </tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_master_verify_checksum" target="_blank">master_verify_checksum</a></td>
      <td><code translate="no" dir="ltr">boolean</code>
        <br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        Default: <code translate="no" dir="ltr">off</code>
        <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">source_verify_checksum</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
      </td>
        <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_allowed_packet" target="_blank">max_allowed_packet</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">16384</code> ... <code translate="no" dir="ltr"> 1073741824</code>
        <p>This value must be a multiple of 1024, if sql_mode=TRADITIONAL or sql_mode=STRICT_ALL_TABLES.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_max_binlog_cache_size" target="_blank">max_binlog_cache_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">4096</code> ... <code translate="no" dir="ltr"> 4294967296</code>
      <br>default: <code translate="no" dir="ltr">4294967296</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_max_binlog_size" target="_blank">max_binlog_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">4096</code> ... <code translate="no" dir="ltr"> 1073741824</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_max_binlog_stmt_cache_size" target="_blank">max_binlog_stmt_cache_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">4096</code> ... <code translate="no" dir="ltr"> 4294967296</code>
      <br>default: <code translate="no" dir="ltr">4294967296</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_connect_errors" target="_blank">max_connect_errors</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code>
      <br>default: <code translate="no" dir="ltr">100</code></td>
      <td>No</td>
</tr>
<tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_connections" target="_blank">max_connections</a></td>
    <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 100000</code>
    </td>
    <td>No</td>
  </tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_digest_length" target="_blank">max_digest_length</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1048576</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_error_count" target="_blank">max_error_count</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 65535</code>
      <br>default: <br>
            MySQL 5.7 or lower: <code translate="no" dir="ltr">64</code><br>
            MySQL 8.0 and later: <code translate="no" dir="ltr">1024</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_execution_time" target="_blank">max_execution_time</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_heap_table_size" target="_blank">max_heap_table_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">16384</code> ... <code translate="no" dir="ltr"> 67108864</code>
        <p>See the <a href="/sql/docs/mysql/flags#tips-heap">Tips</a> section for more information about this flag.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_join_size" target="_blank">max_join_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">16</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_length_for_sort_data" target="_blank">max_length_for_sort_data</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">4</code> ... <code translate="no" dir="ltr"> 8388608</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_points_in_geometry" target="_blank">max_points_in_geometry</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">3</code> ... <code translate="no" dir="ltr"> 1048576</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_prepared_stmt_count" target="_blank">max_prepared_stmt_count</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1048576</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_seeks_for_key" target="_blank">max_seeks_for_key</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_sort_length" target="_blank">max_sort_length</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">4</code> ... <code translate="no" dir="ltr"> 8388608</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_sp_recursion_depth" target="_blank">max_sp_recursion_depth</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 255</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_user_connections" target="_blank">max_user_connections</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 4294967295</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_max_write_lock_count" target="_blank">max_write_lock_count</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_min_examined_row_limit" target="_blank">min_examined_row_limit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 4294967295</code>
      <br>default: <code translate="no" dir="ltr">0</code></td>
      <td>No</td>
</tr>
<tr>
  <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_myisam_data_pointer_size">myisam_data_pointer_size</a></td>
    <td>
      <code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">2...7</code>
      <br> default: <code translate="no" dir="ltr">6</code>
    </td>
      <td>No</td>
</tr>
<tr>
  <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_myisam_max_sort_file_size">myisam_max_sort_file_size</a></td>
    <td>
      <code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">0...9223372036853727232</code>
      <br> default: <code translate="no" dir="ltr">9223372036853727232</code>
    </td>
      <td>No</td>
</tr>
<tr>
  <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_myisam_mmap_size">myisam_mmap_size</a></td>
    <td>
      <code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">7...9223372036854775807</code>
      <br> default: <code translate="no" dir="ltr">9223372036854775807</code>
    </td>
      <td>Yes</td>
</tr>
<tr>
  <td><a id="mysql-c"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_myisam_sort_buffer_size">myisam_sort_buffer_size</a></td>
    <td>
      <code translate="no" dir="ltr">integer</code> <br><code translate="no" dir="ltr">4096...4294967295</code>
      <br> default: <code translate="no" dir="ltr">8388608</code>
    </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_myisam_stats_method" target="_blank">myisam_stats_method</a></td>
      <td><code translate="no" dir="ltr">string</code><br><code translate="no" dir="ltr">"nulls_unequal, nulls_equal, nulls_ignored"</code>
      <br>default: <code translate="no" dir="ltr">nulls_unequal</code></td>
      <td>No</td>
</tr>
<tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_myisam_use_mmap" target="_blank">myisam_use_mmap</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">off</code>
    </td>
      <td>No</td>
</tr>
<tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_mysql_native_password_proxy_users" target="_blank">mysql_native_password_proxy_users</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">off</code>
    </td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-n"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_net_buffer_length" target="_blank">net_buffer_length</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1024</code> ... <code translate="no" dir="ltr"> 1048576</code>
      <br>
      default: <code translate="no" dir="ltr">16384</code>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-n"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_net_read_timeout" target="_blank">net_read_timeout</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">30</code> ... <code translate="no" dir="ltr"> 4294967295</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_net_retry_count" target="_blank">net_retry_count</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">10</code> ... <code translate="no" dir="ltr"> 4294967295</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_net_write_timeout" target="_blank">net_write_timeout</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">60</code> ... <code translate="no" dir="ltr"> 4294967295</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_ngram_token_size" target="_blank">ngram_token_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 10</code>
      <br>
      default: <code translate="no" dir="ltr">2</code>
      </td>
      <td>Yes</td>
</tr>
<tr>
      <td><a id="mysql-o"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_optimizer_prune_level" target="_blank">optimizer_prune_level</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_optimizer_search_depth" target="_blank">optimizer_search_depth</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 62</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_optimizer_switch" target="_blank">optimizer_switch</a></td>
      <td>
      <code translate="no" dir="ltr">multi-value repeated string</code> <br>
      <p>See the <a href="/sql/docs/mysql/flags#tips-multi-value" target="_blank">Tips</a> section for more information about multi-value flags.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_optimizer_trace" target="_blank">optimizer_trace</a></td>
      <td>
      <code translate="no" dir="ltr">multi-value repeated string</code> <br> <code translate="no" dir="ltr">enabled=on</code>, <code translate="no" dir="ltr">enabled=off</code>, <code translate="no" dir="ltr">one_line=on</code>, <code translate="no" dir="ltr">one_line=off</code><br>
      <p>See the <a href="/sql/docs/mysql/flags#tips-multi-value" target="_blank">Tips</a> section for more information about multi-value flags.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-gtids.html#sysvar_binlog_gtid_simple_recovery" target="_blank">optimizer_trace_features</a></td>
      <td>
      <code translate="no" dir="ltr">multi-value repeated string</code> <br>
      <p>See the <a href="/sql/docs/mysql/flags#tips-multi-value" target="_blank">Tips</a> section for more information about multi-value flags.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_optimizer_trace_max_mem_size" target="_blank">optimizer_trace_max_mem_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_optimizer_trace_offset" target="_blank">optimizer_trace_offset</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-9223372036854775808</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-p"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_parser_max_mem_size" target="_blank">parser_max_mem_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">10000000</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_password_history" target="_blank">password_history</a></td>
      <td><code translate="no" dir="ltr">integer</code> <code translate="no" dir="ltr">0-4294967295</code>
        <br>default: <code translate="no" dir="ltr">0</code>
    <br><aside class="note"> <b>Note:  </b>The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
    </td>
      <td>No</td>
  </tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_password_require_current" target="_blank">password_require_current</a></td>
      <td><code translate="no" dir="ltr">boolean</code> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        <br>default: <code translate="no" dir="ltr">off</code>
    <br><aside class="note"> <b>Note:  </b>The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
    </td>
      <td>No</td>
  </tr>
  <tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_password_reuse_interval" target="_blank">password_reuse_interval </a></td>
      <td><code translate="no" dir="ltr">integer</code> <code translate="no" dir="ltr">0-4294967295</code>
        <br>default: <code translate="no" dir="ltr">0</code>
    <br><aside class="note"> <b>Note:  </b>The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside>
    </td>
      <td>No</td>
  </tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema" target="_blank">performance_schema</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        <br>default: <code translate="no" dir="ltr">off</code> for MySQL 5.6, 5.7
        <br>default: <code translate="no" dir="ltr">off</code> for MySQL 8.0 and later
        instances with RAM less than 12&nbsp;GB
        <br>default: <code translate="no" dir="ltr">on</code> for MySQL 8.0 and later
        instances with RAM greater than or equal to 12&nbsp;GB
      <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a>
        section for more information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_accounts_size" target="_blank">performance_schema_accounts_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a>
        section for more information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_digests_size" target="_blank">performance_schema_digests_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a>
        section for more information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_error_size" target="_blank">performance_schema_error_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">1048576</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_events_stages_history_long_size" target="_blank">performance_schema_events_stages_history_long_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_events_stages_history_size" target="_blank">performance_schema_events_stages_history_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1024</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a>
         section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_events_statements_history_long_size" target="_blank">performance_schema_events_statements_history_long_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr">1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_events_statements_history_size" target="_blank">performance_schema_events_statements_history_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1024</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code>flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_events_transactions_history_long_size" target="_blank">performance_schema_events_transactions_history_long_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_events_transactions_history_size" target="_blank">performance_schema_events_transactions_history_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1024</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_events_waits_history_long_size" target="_blank">performance_schema_events_waits_history_long_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_events_waits_history_size" target="_blank">performance_schema_events_waits_history_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1024</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_hosts_size" target="_blank">performance_schema_hosts_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_cond_classes" target="_blank">performance_schema_max_cond_classes</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 256</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_cond_instances" target="_blank">performance_schema_max_cond_instances</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_digest_length" target="_blank">performance_schema_max_digest_length</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_digest_sample_age" target="_blank">
      performance_schema_max_digest_sample_age</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1048576</code><br>
      default: <code translate="no" dir="ltr">60</code>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_file_classes" target="_blank">performance_schema_max_file_classes</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 256</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_file_handles" target="_blank">performance_schema_max_file_handles</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about p<code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_file_instances" target="_blank">performance_schema_max_file_instances</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_index_stat" target="_blank">performance_schema_max_index_stat</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_memory_classes" target="_blank">
      performance_schema_max_memory_classes</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1024</code>
      <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a>
      section for more information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_metadata_locks" target="_blank">performance_schema_max_metadata_locks</a>
      </td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 104857600</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_mutex_classes" target="_blank">performance_schema_max_mutex_classes</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 256</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_mutex_instances" target="_blank">performance_schema_max_mutex_instances</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 104857600</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_prepared_statements_instances" target="_blank">performance_schema_max_prepared_statements_instances</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_program_instances" target="_blank">performance_schema_max_program_instances</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_rwlock_classes" target="_blank">performance_schema_max_rwlock_classes</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 256</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_rwlock_instances" target="_blank">performance_schema_max_rwlock_instances</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 104857600</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_socket_classes" target="_blank">performance_schema_max_socket_classes</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 256</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_socket_instances" target="_blank">performance_schema_max_socket_instances</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr">1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_sql_text_length" target="_blank">performance_schema_max_sql_text_length</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_stage_classes" target="_blank">performance_schema_max_stage_classes</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 256</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_statement_classes" target="_blank">performance_schema_max_statement_classes</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 256</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_statement_stack" target="_blank">performance_schema_max_statement_stack</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 256</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_table_handles" target="_blank">performance_schema_max_table_handles</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_table_instances" target="_blank">performance_schema_max_table_instances</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_table_lock_stat" target="_blank">performance_schema_max_table_lock_stat</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_thread_classes" target="_blank">performance_schema_max_thread_classes</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 256</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_max_thread_instances" target="_blank">performance_schema_max_thread_instances</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_session_connect_attrs_size" target="_blank">performance_schema_session_connect_attrs_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_setup_actors_size" target="_blank">performance_schema_setup_actors_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_setup_objects_size" target="_blank">performance_schema_setup_objects_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_show_processlist" target="_blank">performance_schema_show_processlist</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> Default: <code translate="no" dir="ltr">off</code>
      <br><aside class="note"> <b>Note:  </b>The flag is supported in MySQL 8.0.26 and later
        for Cloud SQL.</aside>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-system-variables.html#sysvar_performance_schema_users_size" target="_blank">performance_schema_users_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">-1</code> ... <code translate="no" dir="ltr"> 1048576</code>
        <p>See <a href="/sql/docs/mysql/flags#tips-performance-schema">Tips</a> section for more
         information about <code translate="no" dir="ltr">performance_schema</code> flags.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_preload_buffer_size" target="_blank">
      preload_buffer_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1024</code> ... <code translate="no" dir="ltr"> 1073741824</code>
      <br>
      default: <code translate="no" dir="ltr">32768</code>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-q"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_query_alloc_block_size" target="_blank">query_alloc_block_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1024</code> ... <code translate="no" dir="ltr"> 4294967295</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html#sysvar_query_cache_limit" target="_blank">query_cache_limit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 223338299392</code><br>
      <p>This flag is not available for MySQL 8.0 and later as the query cache is deprecated as of MySQL 5.7.20, and is removed in MySQL 8.0.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html#sysvar_query_cache_min_res_unit" target="_blank">query_cache_min_res_unit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code><br>
      <p>This flag is not available for MySQL 8.0 and later as the query cache is deprecated as of MySQL 5.7.20, and is removed in MySQL 8.0.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html#sysvar_query_cache_size" target="_blank">query_cache_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 223338299392</code><br>
      <p>This flag is not available for MySQL 8.0 and later as the query cache is deprecated as of MySQL 5.7.20, and is removed in MySQL 8.0.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html#sysvar_query_cache_type" target="_blank">query_cache_type</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 2</code><br>
      <p>This flag is not available for MySQL 8.0 and later as the query cache is deprecated as of MySQL 5.7.20, and is removed in MySQL 8.0.</p></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html#sysvar_query_cache_wlock_invalidate" target="_blank">query_cache_wlock_invalidate</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code><br>
      <p>This flag is not available for MySQL 8.0 and later as the query cache is deprecated as of MySQL 5.7.20, and is removed in MySQL 8.0.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_query_prealloc_size" target="_blank">query_prealloc_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">8192</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-r"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_range_alloc_block_size" target="_blank">range_alloc_block_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">4096</code> ... <code translate="no" dir="ltr"> 4294967295</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_range_optimizer_max_mem_size" target="_blank">range_optimizer_max_mem_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_read_buffer_size" target="_blank">read_buffer_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">8192</code> ... <code translate="no" dir="ltr"> 2147483647</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_read_only" target="_blank">read_only</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        <p>Has no effect for replicas.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_read_rnd_buffer_size" target="_blank">read_rnd_buffer_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 2147483647</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_regexp_stack_limit" target="_blank">regexp_stack_limit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 2147483647</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_regexp_time_limit" target="_blank">regexp_time_limit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 2147483647</code>
      <br>
      default: <code translate="no" dir="ltr">32</code>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_checkpoint_group" target="_blank">replica_checkpoint_group</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">32</code> ... <code translate="no" dir="ltr">524280</code><br>
        Default is 512.
        <p>This flag doesn't affect replicas that don't have multithreading
        enabled.</p>
         <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use the <code translate="no" dir="ltr">slave_checkpoint_group</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_checkpoint_period" target="_blank">replica_checkpoint_period</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">4294967295</code><br>
        Default is 300.
        <p>The unit is milliseconds.</p>
          <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use
          the <code translate="no" dir="ltr">slave_checkpoint_period</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
        </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_compressed_protocol" target="_blank">replica_compressed_protocol</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
          <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use
          the <code translate="no" dir="ltr">slave_compressed_protocol</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
        </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_net_timeout" target="_blank">replica_net_timeout</a></td>
      <td><code translate="no" dir="ltr">integer</code><br>
        <code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">31536000</code>
        <p>The unit is seconds.</p>
                <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use
          the <code translate="no" dir="ltr">slave_net_timeout</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
        </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_parallel_type" target="_blank">replica_parallel_type</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">DATABASE</code>, <code translate="no" dir="ltr">LOGICAL_CLOCK</code><br>
       Default:<br>
      MySQL 8.0.26 or earlier: <code translate="no" dir="ltr">DATABASE</code><br>
      MySQL 8.0.27 or later: <code translate="no" dir="ltr">LOGICAL_CLOCK</code>
      <p>For information about how to use this flag and its acceptable values,
      see <a href="/sql/docs/mysql/replication/manage-replicas#configuring-parallel-replication">Configuring parallel replication</a>.</p>
        <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use
          the <code translate="no" dir="ltr">slave_parallel_type</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
        </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_parallel_workers" target="_blank">replica_parallel_workers</a></td>
      <td><code translate="no" dir="ltr">integer</code>
      <p>For information about how to use this flag and its acceptable values,
      see <a href="/sql/docs/mysql/replication/manage-replicas#configuring-parallel-replication">Configuring parallel replication</a>.</p>
      <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use
          the <code translate="no" dir="ltr">slave_parallel_workers</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_pending_jobs_size_max" target="_blank">replica_pending_jobs_size_max</a></td>
      <td><code translate="no" dir="ltr">integer</code>
      <p>For information about how to use this flag and its acceptable values,
      see <a href="/sql/docs/mysql/replication/manage-replicas#configuring-parallel-replication">Configuring parallel replication</a>.</p>
          <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use
          the <code translate="no" dir="ltr">slave_pending_jobs_size_max</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
        </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_preserve_commit_order" target="_blank">replica_preserve_commit_order</a></td>
      <td><code translate="no" dir="ltr">boolean</code>
      <p>For information about how to use this flag and its acceptable values,
      see <a href="/sql/docs/mysql/replication/manage-replicas#configuring-parallel-replication">Configuring parallel replication</a>.</p>
        <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use
          the <code translate="no" dir="ltr">slave_preserve_commit_order</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_skip_errors" target="_blank">replica_skip_errors</a></td>
      <td><code translate="no" dir="ltr">string</code>
        <br>Default: <code translate="no" dir="ltr">OFF</code>
       <p>For more information about this flag, see the
          <a href="/sql/docs/mysql/flags#tips-event-scheduler" target="_blank">Tips</a>
          section.</p>
          <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use
          the <code translate="no" dir="ltr">slave_skip_errors</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_sql_verify_checksum" target="_blank">replica_sql_verify_checksum</a></td>
      <td><code translate="no" dir="ltr">boolean</code>
        <br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code><br>
          <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use
          the <code translate="no" dir="ltr">slave_sql_verify_checksum</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_transaction_retries" target="_blank">replica_transaction_retries</a></td>
      <td><code translate="no" dir="ltr">integer</code>
        <br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">9223372036854775807</code><br>
         <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use
          the <code translate="no" dir="ltr">slave_transaction_retries</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_replica_type_conversions" target="_blank">replica_type_conversions</a></td>
      <td><code translate="no" dir="ltr">String</code><br>
      values: <code translate="no" dir="ltr">ALL_LOSSY</code>, <code translate="no" dir="ltr">ALL_NON_LOSSY</code>, <code translate="no" dir="ltr">ALL_SIGNED</code>, <code translate="no" dir="ltr">ALL_UNSIGNED</code><br>
         <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use the <code translate="no" dir="ltr">slave_type_conversions</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
      </td>
        <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#option_mysqld_replicate-do-db" target="_blank">replicate_do_db</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <p>For more information about how to use this flag, see the <a href="/sql/docs/mysql/flags#replication-filters">Replication filters</a> section.</td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#option_mysqld_replicate-do-table" target="_blank">replicate_do_table</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <p>For more information about how to use this flag, see the <a href="/sql/docs/mysql/flags#replication-filters">Replication filters</a> section.</td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#option_mysqld_replicate-ignore-db" target="_blank">replicate_ignore_db</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <p>For more information about how to use this flag, see the <a href="/sql/docs/mysql/flags#replication-filters">Replication filters</a> section.</td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#option_mysqld_replicate-ignore-table" target="_blank">replicate_ignore_table</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <p>For more information about how to use this flag, see the <a href="/sql/docs/mysql/flags#replication-filters">Replication filters</a> section.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#option_mysqld_replicate-wild-do-table" target="_blank">replicate_wild_do_table</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <p>For more information about how to use this flag, see the <a href="/sql/docs/mysql/flags#replication-filters">Replication filters</a> section.</p>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#option_mysqld_replicate-wild-ignore-table" target="_blank">replicate_wild_ignore_table</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <p>For more information about how to use this flag, see the <a href="/sql/docs/mysql/flags#replication-filters">Replication filters</a> section.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_rpl_read_size" target="_blank">rpl_read_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">8192</code> ... <code translate="no" dir="ltr"> 4294959104</code>
      <br>
      default: <code translate="no" dir="ltr">8192</code>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-s"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_schema_definition_cache" target="_blank">schema_definition_cache</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">256</code> ... <code translate="no" dir="ltr"> 524288</code>
      <br>
      default: <code translate="no" dir="ltr">256</code>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_session_track_gtids" target="_blank">session_track_gtids</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <br>
      <code translate="no" dir="ltr">OFF</code> | <code translate="no" dir="ltr">OWN_GTID</code> | <code translate="no" dir="ltr">ALL_GTIDS</code>
      <br>
      default: <code translate="no" dir="ltr">OFF</code>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_session_track_schema" target="_blank">session_track_schema</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br>
        <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        <br>
        default: <code translate="no" dir="ltr">on</code>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_session_track_state_change" target="_blank">session_track_state_change</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br>
        <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        <br>
        default: <code translate="no" dir="ltr">off</code>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_session_track_transaction_info" target="_blank">session_track_transaction_info</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <br>
      <code translate="no" dir="ltr">OFF</code> | <code translate="no" dir="ltr">STATE</code> | <code translate="no" dir="ltr">CHARACTERISTICS</code>
      <br>
      default: <code translate="no" dir="ltr">OFF</code>
      </td>
      <td>No</td>
</tr>
<tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_mysql_sha256_password_proxy_users" target="_blank">sha256_password_proxy_users</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">off</code>
    </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_show_create_table_verbosity" target="_blank">show_create_table_verbosity</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        <br>
        default: <code translate="no" dir="ltr">off</code>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html#sysvar_show_compatibility_56" target="_blank">show_compatibility_56</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
        <p>Supported in MySQL 5.7 only.</p></td>
      <td>No</td>
</tr>
<tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en//server-options.html#option_mysqld_character-set-client-handshake" target="_blank">skip_character_set_client_handshake</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">off</code>
    </td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_skip_show_database" target="_blank">skip_show_database</a></td>
      <td><code translate="no" dir="ltr">flag</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_checkpoint_group" target="_blank">slave_checkpoint_group</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">32</code> ... <code translate="no" dir="ltr">524280</code><br>
        Default is 512.
        <p>This flag doesn't affect replicas that don't have multithreading
        enabled.</p>
         <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_checkpoint_group</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_checkpoint_period" target="_blank">slave_checkpoint_period</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">4294967295</code><br>
        Default is 300.
        <p>The unit is milliseconds.</p>
         <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_checkpoint_period</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_compressed_protocol" target="_blank">slave_compressed_protocol</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
       <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_compressed_protocol</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_net_timeout" target="_blank">slave_net_timeout</a></td>
      <td><code translate="no" dir="ltr">integer</code><br>
        <code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr">31536000</code>
        <p>The unit is seconds.</p>
         <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_net_timeout</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_parallel_type" target="_blank">slave_parallel_type</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">DATABASE</code>, <code translate="no" dir="ltr">LOGICAL_CLOCK</code><br>
       Default:<br>
      MySQL 8.0.26 or earlier: <code translate="no" dir="ltr">DATABASE</code><br>
      MySQL 8.0.27 or later: <code translate="no" dir="ltr">LOGICAL_CLOCK</code>
      <p>For information about how to use this flag and its acceptable values,
      see <a href="/sql/docs/mysql/replication/manage-replicas#configuring-parallel-replication">Configuring parallel replication</a>.</p>
      <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_parallel_type</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_parallel_workers" target="_blank">slave_parallel_workers</a></td>
      <td><code translate="no" dir="ltr">integer</code>
      <p>For information about how to use this flag and its acceptable values,
      see <a href="/sql/docs/mysql/replication/manage-replicas#configuring-parallel-replication">Configuring parallel replication</a>.</p>
      <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_parallel_workers</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_preserve_commit_order" target="_blank">slave_preserve_commit_order</a></td>
      <td><code translate="no" dir="ltr">boolean</code>
      <p>For information about how to use this flag and its acceptable values,
      see <a href="/sql/docs/mysql/replication/manage-replicas#configuring-parallel-replication">Configuring parallel replication</a>.</p>
       <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_preserve_commit_order</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_pending_jobs_size_max" target="_blank">slave_pending_jobs_size_max</a></td>
      <td><code translate="no" dir="ltr">integer</code>
      <p>For information about how to use this flag and its acceptable values,
      see <a href="/sql/docs/mysql/replication/manage-replicas#configuring-parallel-replication">Configuring parallel replication</a>.</p>
       <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_pending_jobs_size_max</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_skip_errors" target="_blank">slave_skip_errors</a></td>
      <td><code translate="no" dir="ltr">string</code>
        <br>Default: <code translate="no" dir="ltr">OFF</code>
       <p>For more information about this flag, see the
          <a href="/sql/docs/mysql/flags#tips-event-scheduler" target="_blank">Tips</a>
          section.</p>
          <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_skip_errors</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_sql_verify_checksum" target="_blank">slave_sql_verify_checksum</a></td>
      <td><code translate="no" dir="ltr">boolean</code>
        <br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
             <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_sql_verify_checksum</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_transaction_retries" target="_blank">slave_transaction_retries</a></td>
      <td><code translate="no" dir="ltr">integer</code>
        <br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr">9223372036854775807</code><br>
      <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_transaction_retries</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_slave_type_conversions" target="_blank">slave_type_conversions</a></td>
      <td><code translate="no" dir="ltr">string</code><br>
      values: <code translate="no" dir="ltr">ALL_LOSSY</code>, <code translate="no" dir="ltr">ALL_NON_LOSSY</code>, <code translate="no" dir="ltr">ALL_SIGNED</code>, <code translate="no" dir="ltr">ALL_UNSIGNED</code><br>
        <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">replica_type_conversions</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
      </td>
        <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_slow_launch_time" target="_blank">slow_launch_time</a></td>
      <td><code translate="no" dir="ltr">Integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 31536000</code><br>
      Default: <code translate="no" dir="ltr">2</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/slow-query-log.html" target="_blank">slow_query_log</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code><br>
      <p>See the <a href="/sql/docs/mysql/flags#tips-general-log" target="_blank">Tips</a> section for more information on slow query logs.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_sort_buffer_size" target="_blank">sort_buffer_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">32768</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_source_verify_checksum" target="_blank">source_verify_checksum</a></td>
      <td><code translate="no" dir="ltr">boolean</code>
        <br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code><br>
        Default: <code translate="no" dir="ltr">off</code>
         <aside class="note"><b>Note</b>: This flag name is available for MySQL 8.0.26 and later only. For MySQL versions 8.0.18 and earlier, use the <code translate="no" dir="ltr">master_verify_checksum</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
      </td>
        <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_sql_generate_invisible_primary_key" target="_blank">sql_generate_invisible_primary_key</a></td>
      <td><code translate="no" dir="ltr">boolean</code>
        <br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code><br>
        Default: <code translate="no" dir="ltr">off</code>
         <br><aside class="note"> <b>Note:  </b>The flag is supported in MySQL 8.0.30 and later
        for Cloud SQL.</aside>
      </td>
        <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_sql_mode" target="_blank">sql_mode</a></td>
      <td><code translate="no" dir="ltr">string</code>
      <p>See the <a href="https://dev.mysql.com/doc/refman/8.0/en/sql-mode.html" target="_blank">Server SQL Modes</a>
        in the MySQL documentation for allowed values, including combined modes,
        such as <code translate="no" dir="ltr">ANSI</code>. <code translate="no" dir="ltr">NO_DIR_IN_CREATE</code> is not
        supported.</p>
        <p>Cloud SQL for MySQL doesn't support empty values for the
          <code translate="no" dir="ltr">sql_mode</code> flag. Instead of using an empty value, set this
          flag to the
          <a href="https://dev.mysql.com/doc/refman/8.0/en/sql-mode.html#sqlmode_no_engine_substitution"><code translate="no" dir="ltr">NO_ENGINE_SUBSTITUTION</code></a>
          mode.</p>
  <aside class="note">If specifying a comma-separated list of modes using
    <a href="/sdk/gcloud">gcloud CLI</a> commands, then use the <a href="/sdk/gcloud/reference#--flags-file">--flags-file</a> argument.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_sql_require_primary_key" target="_blank">sql_require_primary_key</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code><br>
      Default: <code translate="no" dir="ltr">off</code></td>
      <td>No</td>
</tr>
<tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/enserver-system-variables.html#sysvar_sql_select_limit" target="_blank">sql_select_limit</a></td>
    <td><code translate="no" dir="ltr">integer</code> <code translate="no" dir="ltr">0...18446744073709551615</code>
      <br> default: <code translate="no" dir="ltr">18446744073709551615</code>
       <p>See the <a href="/sql/docs/mysql/flags#tips-option-set" target="_blank">Tips</a> section for more information about this flag.</p>
       <aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0
        for Cloud SQL and later.</aside></td>
    <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_stored_program_cache" target="_blank">stored_program_cache</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">16</code> ... <code translate="no" dir="ltr"> 524288</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_stored_program_definition_cache" target="_blank">stored_program_definition_cache</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">256</code> ... <code translate="no" dir="ltr"> 524288</code><br>
      Default: <code translate="no" dir="ltr">256</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-binary-log.html#sysvar_sync_binlog" target="_blank">sync_binlog</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 4294967295</code>
        <p>The default setting of 1 enables the synchronization of the binary
          log to disk before transactions are committed.</p>
        <p>If you promote a replica with this flag enabled,
        the flag is automatically removed causing the promoted replica to have full durability
        by default. To use this flag with a promoted replica, you can update the flag to
        the replica after promotion.</p>
        <aside class="note"> <b>Note</b>: Changing the default value for the <code translate="no" dir="ltr">sync_binlog</code>
        flag will cause the instance to lose SLA coverage as it may reduce durability of the instance's data.</aside>
        <p>See the <a href="/sql/docs/mysql/flags#tips-flush-log">Tips</a> section for more information about this flag.</p></td>
      </td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_sync_master_info" target="_blank">sync_master_info</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 4294967295</code><br>
      Default: <code translate="no" dir="ltr">10000</code>
      <aside class="note">This flag name is deprecated for MySQL 8.0.26 and later. Instead, use the <code translate="no" dir="ltr">sync_source_info</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_sync_relay_log" target="_blank">sync_relay_log</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 4294967295</code><br>
      Default: <code translate="no" dir="ltr">10000</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_sync_relay_log_info" target="_blank">sync_relay_log_info</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 4294967295</code><br>
      Default: <code translate="no" dir="ltr">10000</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_sync_source_info" target="_blank">sync_source_info</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 4294967295</code><br>
      Default: <code translate="no" dir="ltr">10000</code>
          <aside class="note">This flag name is only available for MySQL 8.0.26 and later. For MySQL versions 8.0.18 and earlier, use the <code translate="no" dir="ltr">sync_master_info</code> flag. For more information, see <a href="/sql/docs/mysql/flags#aliased-flags">Aliased flags</a>.</aside>
      </td>
      <td>No</td>
</tr>
<tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-options.html#option_mysqld_sysdate-is-now" target="_blank">sysdate_is_now</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">off</code>
    </td>
      <td>Yes</td>
</tr>
<tr>
      <td><a id="mysql-t"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_table_definition_cache" target="_blank">table_definition_cache</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">400</code> ... <code translate="no" dir="ltr"> 524288</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_tablespace_definition_cache" target="_blank">tablespace_definition_cache</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">256</code> ... <code translate="no" dir="ltr"> 524288</code><br>
      Default: <code translate="no" dir="ltr">256</code></td>
      <td>No</td>
</tr>
<tr>
        <td><a id="table-open-cache"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_table_open_cache" target="_blank">table_open_cache</a></td>
        <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 524288</code></td>
        <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_table_open_cache_instances" target="_blank">table_open_cache_instances</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 64</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_temptable_max_mmap" target="_blank">temptable_max_mmap</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 68719476736</code><br>
      Default: <code translate="no" dir="ltr">1073741824</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_temptable_max_ram" target="_blank">temptable_max_ram</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">2097152</code> ... <code translate="no" dir="ltr"> 68719476736</code><br>
      Default: <code translate="no" dir="ltr">1073741824</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_thread_cache_size" target="_blank">thread_cache_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 16384</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_thread_stack" target="_blank">thread_stack</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">131072</code> ... <code translate="no" dir="ltr"> 9223372036854775807</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_tls_version" target="_blank">tls_version</a></td>
      <td><code translate="no" dir="ltr">String</code><br><br>
      <b>Version 5.7 to Version 8.0.27:</b><code translate="no" dir="ltr"> TLSv1, TLSv1.1</code><br>


      <b>Version 8.0.28 or later: </b><code translate="no" dir="ltr">TLSv1.2</code><br>

      </td>
      <td><b>Version 5.7</b>: Yes<br><br>
      <b>Version 8.0 or later</b>: No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_tmp_table_size" target="_blank">tmp_table_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1024</code> ... <code translate="no" dir="ltr"> 67108864</code>
        <p>See the
        <a href="/sql/docs/mysql/flags#tips-heap" target="_blank">Tips</a>
        section for more information about this flag.</p></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_transaction_alloc_block_size" target="_blank">transaction_alloc_block_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1024</code> ... <code translate="no" dir="ltr"> 131072</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_transaction_isolation" target="_blank">transaction_isolation</a></td>
      <td><code translate="no" dir="ltr">enumeration</code><br><code translate="no" dir="ltr">READ-UNCOMMITTED</code> | <code translate="no" dir="ltr">READ-COMMITTED</code> | <code translate="no" dir="ltr">REPEATABLE-READ</code> | <code translate="no" dir="ltr">SERIALIZABLE</code></td>
      <td>Yes</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_transaction_prealloc_size" target="_blank">transaction_prealloc_size</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1024</code> ... <code translate="no" dir="ltr"> 131072</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_transaction_write_set_extraction" target="_blank">transaction_write_set_extraction</a></td>
      <td><code translate="no" dir="ltr">enumeration</code>
      <p>For information about how to use this flag and its acceptable values,
      see <a href="/sql/docs/mysql/replication/manage-replicas#configuring-parallel-replication">Configuring parallel replication</a>. This flag is not supported in MySQL 8.4.</p></td>
      <td>No</td>
</tr>
<tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en//server-system-variables.html#sysvar_unique_checks" target="_blank">unique_checks</a></td>
    <td>
      <code translate="no" dir="ltr">boolean</code> <br> <code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code>
      <br> default: <code translate="no" dir="ltr">on</code>
     <p>See the <a href="/sql/docs/mysql/flags#tips-option-set" target="_blank">Tips</a> section for more information about this flag.</p>
     <aside class="note"> <b>Note:</b> The flag is supported in MySQL 8.0 and later
        for Cloud SQL.</aside></td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-u"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_updatable_views_with_limit" target="_blank">updatable_views_with_limit</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">0</code> ... <code translate="no" dir="ltr"> 1</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a id="mysql-w"></a><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_wait_timeout" target="_blank">wait_timeout</a></td>
      <td><code translate="no" dir="ltr">integer</code><br><code translate="no" dir="ltr">1</code> ... <code translate="no" dir="ltr"> 31536000</code></td>
      <td>No</td>
</tr>
<tr>
      <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_windowing_use_high_precision" target="_blank">windowing_use_high_precision</a></td>
      <td><code translate="no" dir="ltr">boolean</code><br><code translate="no" dir="ltr">on</code> | <code translate="no" dir="ltr">off</code><br>
      Default: <code translate="no" dir="ltr">on</code>
      </td>
      <td>No</td>
</tr>
</table>

<h3 id="timezone-names" data-text="Timezone names" tabindex="-1">Timezone names</h3>

<p>In this section, you&#39;ll learn about the time-zone names that Cloud SQL for MySQL supports.</p>

<p>The table in this section displays the following:</p>

<ul>
<li>Timezone name: The name that Cloud SQL for MySQL supports.</li>
<li>STD: The time-zone offset in standard time (STD).</li>
<li>DST: The time-zone offset in daylight savings time (DST).</li>
<li>Synonym names: The names for time zones that you may want to use, but they aren&#39;t supported by Cloud SQL for MySQL. If this situation occurs, then use the corresponding time-zone name.</li>
</ul>

<aside class="note"><p>Time-zone names are case insensitive. You can supply the time-zone name in any case. The format for the STD and DST time-zone offsets is <code translate="no" dir="ltr">+/-hh:mm</code>, and the offsets are in UTC. Not all time-zone names have corresponding synonym names. If this occurs, then use the time-zone name.</p></aside>

<table>
  <tr>
    <th>time-zone name</th>
    <th>STD</th>
    <th>DST</th>
    <th>Synonym names</th>
  </tr>
  <tr>
    <td>Africa/Cairo</td>
    <td>+02:00</td>
    <td>+02:00</td>
    <td>Egypt</td>
  </tr>
  <tr>
    <td>Africa/Casablanca</td>
    <td>+01:00</td>
    <td>+00:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Africa/Harare</td>
    <td>+02:00</td>
    <td>+02:00</td>
    <td>Africa/Maputo</td>
  </tr>
  <tr>
    <td>Africa/Monrovia</td>
    <td>+00:00</td>
    <td>+00:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Africa/Nairobi</td>
    <td>+03:00</td>
    <td>+03:00</td>
    <td>Africa/Addis_Ababa
    <br>Africa/Asmera
    <br>Africa/Dar_es_Salaam
    <br>Africa/Djibouti
    <br>Africa/Kampala
    <br>Africa/Mogadishu
    <br>Indian/Antananarivo
    <br>Indian/Comoro
    <br>Indian/Mayotte</td>
  </tr>
  <tr>
    <td>Africa/Tripoli</td>
    <td>+02:00</td>
    <td>+02:00</td>
    <td>Libya</td>
  </tr>
  <tr>
    <td>Africa/Windhoek</td>
    <td>+02:00</td>
    <td>+02:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Araguaina</td>
    <td>−03:00</td>
    <td>−03:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Asuncion</td>
    <td>−04:00</td>
    <td>−03:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Bogota</td>
    <td>−05:00</td>
    <td>−05:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Buenos_Aires</td>
    <td>−03:00</td>
    <td>−03:00</td>
    <td>America/Argentina/Buenos_Aires</td>
  </tr>
  <tr>
    <td>America/Caracas</td>
    <td>−04:00</td>
    <td>−04:00</td>
    <td></td>
  </tr>
    <tr>
    <td>America/Chicago</td>
    <td>−06:00</td>
    <td>−05:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Chihuahua</td>
    <td>−07:00</td>
    <td>−06:00</td>
    <td>America/Ojinaga</td>
  </tr>
  <tr>
    <td>America/Cuiaba</td>
    <td>−04:00</td>
    <td>−04:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Denver</td>
    <td>−07:00</td>
    <td>−06:00</td>
    <td>America/Shiprock
    <br>Navajo
    <br>MST7MDT
    <br>US/Mountain</td>
  </tr>
    <tr>
    <td>America/Detroit</td>
    <td>−05:00</td>
    <td>−04:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Fortaleza</td>
    <td>−03:00</td>
    <td>−03:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Guatemala</td>
    <td>−06:00</td>
    <td>−06:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Halifax</td>
    <td>−04:00</td>
    <td>−03:00</td>
    <td>Canada/Atlantic</td>
  </tr>
   <tr>
    <td>America/Los_Angeles</td>
    <td>−08:00</td>
    <td>−07:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Manaus</td>
    <td>−04:00</td>
    <td>−04:00</td>
    <td>Brazil/West</td>
  </tr>
  <tr>
    <td>America/Matamoros</td>
    <td>−06:00</td>
    <td>−05:00</td>
    <td></td>
  </tr>
     <tr>
    <td>America/Mexico_City</td>
    <td>−06:00</td>
    <td>−05:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Monterrey</td>
    <td>−06:00</td>
    <td>−05:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Montevideo</td>
    <td>−03:00</td>
    <td>−03:00</td>
    <td></td>
  </tr>
   <tr>
    <td>America/New_York</td>
    <td>−05:00</td>
    <td>−04:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Phoenix</td>
    <td>−07:00</td>
    <td>−07:00</td>
    <td>US/Arizona
    <br>MST
    <br>America/Creston</td>
  </tr>
  <tr>
    <td>America/Santiago</td>
    <td>−04:00</td>
    <td>−03:00</td>
    <td>Chile/Continental</td>
  </tr>
  <tr>
    <td>America/Sao_Paolo</td>
    <td>−03:00</td>
    <td>−03:00</td>
    <td></td>
  </tr>
  <tr>
    <td>America/Tijuana</td>
    <td>−08:00</td>
    <td>−07:00</td>
    <td>Mexico/BajaNorte
      <br>America/Ensenada
      <br>America/Santa_Isabel</td>
  </tr>
  <tr>
    <td>Asia/Amman</td>
    <td>+02:00</td>
    <td>+03:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Ashgabat</td>
    <td>+05:00</td>
    <td>+05:00</td>
    <td>Asia/Ashkhabad</td>
  </tr>
  <tr>
    <td>Asia/Baghdad</td>
    <td>+03:00</td>
    <td>+03:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Baku</td>
    <td>+04:00</td>
    <td>+04:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Bangkok</td>
    <td>+07:00</td>
    <td>+07:00</td>
    <td>Asia/Phnom_Penh
      <br>Asia/Vientiane</td>
  </tr>
  <tr>
    <td>Asia/Beirut</td>
    <td>+02:00</td>
    <td>+03:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Calcutta</td>
    <td>+05:30</td>
    <td>+05:30</td>
    <td>Asia/Kolkata</td>
  </tr>
  <tr>
    <td>Asia/Damascus</td>
    <td>+02:00</td>
    <td>+03:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Dhaka</td>
    <td>+06:00</td>
    <td>+06:00</td>
    <td>Asia/Dacca</td>
  </tr>
  <tr>
    <td>Asia/Irkutsk</td>
    <td>+08:00</td>
    <td>+08:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Jerusalem</td>
    <td>+02:00</td>
    <td>+03:00</td>
    <td>Asia/Tel_Aviv
      <br>Israel</td>
  </tr>
  <tr>
    <td>Asia/Kabul</td>
    <td>+04:30</td>
    <td>+04:30</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Karachi</td>
    <td>+05:00</td>
    <td>+05:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Kathmandu</td>
    <td>+05:45</td>
    <td>+05:45</td>
    <td>Asia/Katmandu</td>
  </tr>
    <tr>
    <td>Asia/Kolkata</td>
    <td>+05:30</td>
    <td>+05:30</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Krasnoyarsk</td>
    <td>+07:00</td>
    <td>+07:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Magadan</td>
    <td>+11:00</td>
    <td>+11:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Muscat</td>
    <td>+04:00</td>
    <td>+04:00</td>
    <td>Asia/Dubai</td>
  </tr>
  <tr>
    <td>Asia/Novosibirsk</td>
    <td>+07:00</td>
    <td>+07:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Riyadh</td>
    <td>+03:00</td>
    <td>+03:00</td>
    <td>Asia/Kuwait
      <br>Antarctica/Syowa
      <br>Asia/Aden</td>
  </tr>
  <tr>
    <td>Asia/Seoul</td>
    <td>+09:00</td>
    <td>+09:00</td>
    <td>ROK</td>
  </tr>
  <tr>
    <td>Asia/Shanghai</td>
    <td>+08:00</td>
    <td>+08:00</td>
    <td>Asia/Chongqing
      <br>Asia/Chungking
      <br>Asia/Harbin
      <br>PRC</td>
  </tr>
  <tr>
    <td>Asia/Singapore</td>
    <td>+08:00</td>
    <td>+08:00</td>
    <td>Singapore</td>
  </tr>
  <tr>
    <td>Asia/Taipei</td>
    <td>+08:00</td>
    <td>+08:00</td>
    <td>ROC</td>
  </tr>
  <tr>
    <td>Asia/Tehran</td>
    <td>+03:30</td>
    <td>+04:30</td>
    <td>Iran</td>
  </tr>
  <tr>
    <td>Asia/Tokyo</td>
    <td>+09:00</td>
    <td>+09:00</td>
    <td>Japan</td>
  </tr>
  <tr>
    <td>Asia/Ulaanbaatar</td>
    <td>+08:00</td>
    <td>+08:00</td>
    <td>Asia/Ulan_Bator</td>
  </tr>
  <tr>
    <td>Asia/Vladivostok</td>
    <td>+10:00</td>
    <td>+10:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Yakutsk</td>
    <td>+09:00</td>
    <td>+09:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Asia/Yerevan</td>
    <td>+04:00</td>
    <td>+04:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Atlantic/Azores</td>
    <td>−01:00</td>
    <td>+00:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Australia/Adelaide</td>
    <td>+09:30</td>
    <td>+10:30</td>
    <td>Australia/South</td>
  </tr>
  <tr>
    <td>Australia/Brisbane</td>
    <td>+10:00</td>
    <td>+10:00</td>
    <td>Australia/Queensland</td>
  </tr>
  <tr>
    <td>Australia/Darwin</td>
    <td>+09:30</td>
    <td>+09:30</td>
    <td>Australia/North</td>
  </tr>
  <tr>
    <td>Australia/Hobart</td>
    <td>+10:00</td>
    <td>+11:00</td>
    <td>Australia/Currie
      <br>Australia/Tasmania</td>
  </tr>
  <tr>
    <td>Australia/Perth</td>
    <td>+08:00</td>
    <td>+08:00</td>
    <td>Australia/West</td>
  </tr>
  <tr>
    <td>Australia/Sydney</td>
    <td>+10:00</td>
    <td>+11:00</td>
    <td>Australia/NSW
      <br>Australia/ACT
      <br>Australia/Canberra</td>
  </tr>
  <tr>
    <td>Brazil/East</td>
    <td>−03:00</td>
    <td>−03:00</td>
    <td>America/Sao_Paulo</td>
  </tr>
  <tr>
    <td>Canada/Newfoundland</td>
    <td>−03:30</td>
    <td>−02:30</td>
    <td>America/St_Johns</td>
  </tr>
  <tr>
    <td>Canada/Saskatchewan</td>
    <td>−06:00</td>
    <td>−06:00</td>
    <td>America/Regina</td>
  </tr>
  <tr>
    <td>Canada/Yukon</td>
    <td>−07:00</td>
    <td>−07:00</td>
    <td>America/Whitehorse</td>
  </tr>
  <tr>
    <td>Europe/Amsterdam</td>
    <td>+01:00</td>
    <td>+02:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Europe/Athens</td>
    <td>+02:00</td>
    <td>+03:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Europe/Dublin</td>
    <td>+01:00</td>
    <td>+00:00</td>
    <td>Eire</td>
  </tr>
  <tr>
    <td>Europe/Helsinki</td>
    <td>+02:00</td>
    <td>+03:00</td>
    <td>Europe/Mariehamn</td>
  </tr>
  <tr>
    <td>Europe/Istanbul</td>
    <td>+03:00</td>
    <td>+03:00</td>
    <td>Turkey
      <br>Asia/Istanbul</td>
  </tr>
  <tr>
    <td>Europe/Kaliningrad</td>
    <td>+02:00</td>
    <td>+02:00</td>
    <td></td>
  </tr>
    <tr>
    <td>Europe/Madrid</td>
    <td>+01:00</td>
    <td>+02:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Europe/Moscow</td>
    <td>+03:00</td>
    <td>+03:00</td>
    <td>W-SU</td>
  </tr>
  <tr>
    <td>Europe/Paris</td>
    <td>+01:00</td>
    <td>+02:00</td>
    <td>MET
      <br>CET</td>
  </tr>
  <tr>
    <td>Europe/Prague</td>
    <td>+01:00</td>
    <td>+02:00</td>
    <td>Europe/Bratislava</td>
  </tr>
  <tr>
    <td>Europe/Sarajevo</td>
    <td>+01:00</td>
    <td>+02:00</td>
    <td>Europe/Belgrade
      <br>Europe/Ljubljana
      <br>Europe/Podgorica
      <br>Europe/Skopje
      <br>Europe/Zagreb</td>
  </tr>
  <tr>
    <td>Pacific/Auckland</td>
    <td>+12:00</td>
    <td>+13:00</td>
    <td>NZ
      <br>Antarctica/McMurdo
      <br>Antarctica/South_Pole</td>
  </tr>
  <tr>
    <td>Pacific/Fiji</td>
    <td>+12:00</td>
    <td>+13:00</td>
    <td></td>
  </tr>
  <tr>
    <td>Pacific/Guam</td>
    <td>+10:00</td>
    <td>+10:00</td>
    <td>Pacific/Saipan</td>
  </tr>
  <tr>
    <td>Pacific/Honolulu</td>
    <td>−10:00</td>
    <td>−10:00</td>
    <td>US/Hawaii
      <br>Pacific/Johnston
      <br>HST</td>
  </tr>
  <tr>
    <td>Pacific/Samoa</td>
    <td>−11:00</td>
    <td>−11:00</td>
    <td>Pacific/Pago_Pago
      <br>US/Samoa</td>
  </tr>
  <tr>
    <td>US/Alaska</td>
    <td>−09:00</td>
    <td>−08:00</td>
    <td>America/Anchorage
      <br>America/Juneau
      <br>America/Metlakatla
      <br>America/Nome
      <br>America/Sitka
      <br>America/Yakutat</td>
  </tr>
  <tr>
    <td>US/Central</td>
    <td>−06:00</td>
    <td>−05:00</td>
    <td>America/Chicago</td>
  </tr>
  <tr>
    <td>US/Eastern</td>
    <td>−05:00</td>
    <td>−04:00</td>
    <td>America/New_York</td>
  </tr>
  <tr>
    <td>US/East-Indiana</td>
    <td>−05:00</td>
    <td>−04:00</td>
    <td>America/Indiana/Indianapolis
      <br>America/Indianapolis
      <br>America/Fort_Wayne</td>
  </tr>
  <tr>
    <td>US/Mountain</td>
    <td>−07:00</td>
    <td>−06:00</td>
    <td>America/Denver</td>
  </tr>
  <tr>
    <td>US/Pacific</td>
    <td>−08:00</td>
    <td>−07:00</td>
    <td>America/Los_Angeles</td>
  </tr>
  <tr>
    <td>UTC</td>
    <td>+00:00</td>
    <td>+00:00</td>
    <td>Etc/UTC
      <br>Etc/UCT
      <br>Etc/Universal
      <br>Etc/Zulu</td>
  </tr>
</table>

<p>Timezone tables in Cloud SQL might need refreshing with the latest data. For example, a country might shift from a DST timezone offset to an STD offset or
a country might introduce a new timezone.</p>

<p>For every critical service agent (CSA) release for Cloud SQL, timezone tables are refreshed with the latest data. When this happens, during the non-maintenance window, the replica instances are refreshed. Primary instances are then refreshed during the maintenance window.</p>

<p>You can either wait until the regular maintenance window for the CSA release or you can perform <a href="/sql/docs/mysql/self-service-maintenance">self service maintenance</a> to refresh the timezone tables with the latest data. For more information about viewing the available maintenance versions, see <a href="/sql/docs/mysql/self-service-maintenance#determine-target-maintenance-version">Determine the target maintenance version</a>.</p>
<aside class="note"><strong>Note:</strong><span> For MySQL 8.0 and later, the timezone tables are read-only. You can write to timezone tables for MySQL 5.6 and 5.7. However, we don&#39;t recommend that you write to, or delete from, these tables because this can cause issues with the replica instances.</span></aside>
<h2 id="tips" data-text="Tips for working with flags" tabindex="-1">Tips for working with flags</h2>

<dl>
<dt id="tips-general-log">general_log, slow_query_log</dt>
<dd>
<p>To make your <code translate="no" dir="ltr">general</code> or <code translate="no" dir="ltr">slow query</code> logs available,
  enable the corresponding flag and set the <code translate="no" dir="ltr">log_output</code> flag to
  <code translate="no" dir="ltr">FILE</code>. 
This makes the log output available using the
<a href="/logging/docs/view/logs_viewer">Logs Viewer in the Google Cloud console</a>.
Note that <a href="/stackdriver/pricing">Google Cloud Observability logging charges</a> apply.
 To minimize instance storage cost,
  <code translate="no" dir="ltr">general</code> and <code translate="no" dir="ltr">slow query</code> logs on the instance disk are
  rotated when the log file is older than 24 hours (and no changes have been
  made within that duration) or greater than 100MB in size. Old log files are
  automatically deleted after the rotation.</p>
<p>If the <code translate="no" dir="ltr">log_output</code> is set to <code translate="no" dir="ltr">NONE</code>, you can't
  access the logs. If you set <code translate="no" dir="ltr">log_output</code> to <code translate="no" dir="ltr">TABLE</code>, the
  log output is placed in a table in the mysql system database. It might consume
  a considerable amount of disk space. If this table becomes large, it can
  affect instance restart time or cause the instance to lose its SLA coverage.
  For this reason, the <code translate="no" dir="ltr">TABLE</code> option is not recommended. In
  addition, the log content isn't available in
  <a href="/logging/docs/view/logs-explorer-interface">Logs Explorer</a> and it isn't rotated. If
  needed, you can truncate your log tables by using the API. For more
  information, see the
<a href="/sql/docs/mysql/admin-api/rest/v1beta4/instances/truncateLog" target="_blank">instances.truncateLog reference page</a>.</p>
  <p>
  <aside class="note">MySQL provides a more sophisticated utility for examining
    and processing binary log files called <a href="https://dev.mysql.com/doc/refman/8.0/en/mysqlbinlog.html" target="_blank"><code translate="no" dir="ltr">mysqlbinlog</code></a>. This utility is available with the MySQL Server
    software. If you have a MySQL instance, then you can use
    <code translate="no" dir="ltr">mysqlbinlog</code> to determine your desired recovery position.
  </aside>
  </p>
</dd>
  </dl>

<dl>
<dt id="tips-expire-logs">expire_logs_days, binlog_expire_logs_seconds</dt>
<dd>If you enable point-in-time recovery, the expiration period of your binary
logs is determined by the lesser of your transaction log retention period
and the values of these flags. You can use these flags to manage how long
binary logs are stored on your replicas. The <code translate="no" dir="ltr">expire_logs_days</code>
flag is removed from MySQL 8.4 and later. For more information,
see the <a href="/sql/docs/mysql/backup-recovery/backups#retention">
transaction log retention</a> page.</dd>
</dl>

<dl>
<dt id="tips-buffer-pool">innodb_buffer_pool_size</dt>
<dd><p>The value of this flag is the size in bytes of the buffer pool. The buffer pool size must always be equal to or a multiple of the value that you get when you multiply <code translate="no" dir="ltr">innodb_buffer_pool_chunk_size</code> by <code translate="no" dir="ltr">innodb_buffer_pool_instances</code>. If you alter the
  buffer pool size to a value that's not equal to or a multiple of
  <code translate="no" dir="ltr">innodb_buffer_pool_chunk_size</code> multiplied by <code translate="no" dir="ltr">innodb_buffer_pool_instances</code>, then Cloud SQL adjusts the buffer pool size automatically. You can't enable this flag on instances that have fewer than
  3,840 MiB of RAM.</p>
  <p>You can't configure this flag for shared-core machine types (f1_micro and
  g1_small). Changing this flag on MySQL 5.6 requires a restart.</p>
  <p>In Cloud SQL, the default, minimum allowable, and maximum allowable
  values of the innodb_buffer_pool_size flag depend on the instance's memory.
  These values can be roughly calculated as a percentage of the instance's RAM.
  By default, the value of this flag is typically set close to the maximum
  allowable value. The maximum allowable allocation percentage increases with
  instance size. The minimum allowable value is usually about 20% of the
  instance's RAM.</p>
  <p><b>Approximate values for this flag:</b></p>

 <table>
  <tr><th>Instance RAM Range</th><th>Min %</th><th>Default %</th><th>Max %</th></tr>
  <tr><td>0 - 4.0GB of RAM</td><td></td><td>~34%</td><td></td></tr>
  <tr><td>4.0GB - 7.5GB</td><td>~20%</td><td>~34%</td><td>~34%</td></tr>
  <tr><td>7.5GB - 12GB</td><td>~20%</td><td>~52%</td><td>~52%</td></tr>
  <tr><td>12GB - 24GB</td><td>~20%</td><td>~67%</td><td>~67%</td></tr>
  <tr><td>24GB and above</td><td>~20%</td><td>~72%</td><td>~72%</td></tr>
 </table>
  <p>Your exact values may vary. When <a href="/sql/docs/mysql/optimize-high-memory-usage#enable-managed-buffer-pool">managed buffer pool</a>
  is enabled, it makes adjustments to the <code translate="no" dir="ltr">innodb_buffer_pool_size</code> that are not
  reflected in Google Cloud console. To calculate the current value for your
  instance, you can run the query:</p>

  <div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="SQL"><span class="devsite-syntax-w">  </span><span class="devsite-syntax-k">show</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">global</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-n">variables</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">like</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-s1">'innodb_buffer_pool_size'</span><span class="devsite-syntax-p">;</span>
<span class="devsite-syntax-w">  </span></pre></devsite-code>

  <p>For reference, the minimum allowable, default, and maximum allowable values
  are provided for the machine types below.</p>

<table>
  <tr>
   <td><strong>Machine type</strong></td>
   <td><strong>Instance RAM (GB)</strong></td>
   <td><strong>Min (GB)<br></strong>(% of total)</td>
   <td><strong>Default (GB)<br></strong>(% of total)</td>
   <td><strong>Max (GB)<br></strong>(% of total)</td>
  </tr>
  <tr>
   <td>db-f1-micro</td>
   <td>0.6</td>
   <td>-</td>
   <td>0.053</td>
   <td>-</td>
  </tr>
  <tr>
   <td>db-g1-small</td>
   <td>1.7</td>
   <td>-</td>
   <td>0.625</td>
   <td>-</td>
  </tr>
  <tr>
   <td>db-custom-1-3840</td>
   <td>3.75</td>
   <td>0.875<br>(23%)</td>
   <td>1.375<br>(37%)</td>
   <td>1.375<br>(37%)</td>
  </tr>
  <tr>
   <td>db-custom-2-7680</td>
   <td>7.5</td>
   <td>1.5<br>(20%)</td>
   <td>4<br>(53%)</td>
   <td>4<br>(53%)</td>
  </tr>
  <tr>
   <td>db-custom-4-15360</td>
   <td>15</td>
   <td>3<br>(20%)</td>
   <td>10.5<br>(70%)</td>
   <td>10.5<br>(70%)</td>
  </tr>
  <tr>
   <td>db-custom-8-30720</td>
   <td>30</td>
   <td>6<br>(20%)</td>
   <td>22<br>(73%)</td>
   <td>22<br>(73%)</td>
  </tr>
  <tr>
   <td>db-custom-16-61440</td>
   <td>60</td>
   <td>12<br>(20%)</td>
   <td>44<br>(73%)</td>
   <td>44<br>(73%)</td>
  </tr>
  <tr>
   <td>db-custom-32-122880</td>
   <td>120</td>
   <td>24<br>(20%)</td>
   <td>87<br>(73%)</td>
   <td>87<br>(73%)</td>
  </tr>
  <tr>
   <td>db-custom-64-245760</td>
   <td>240</td>
   <td>48<br>(20%)</td>
   <td>173<br>(72%)</td>
   <td>173<br>(72%)</td>
  </tr>
  <tr>
   <td>db-custom-96-368640</td>
   <td>360</td>
   <td>72<br>(20%)</td>
   <td>260<br>(72%)</td>
   <td>260<br>(72%)</td>
  </tr>
  <tr>
   <td>db-custom-2-13312</td>
   <td>13</td>
   <td>3<br>(23%)</td>
   <td>9<br>(69%)</td>
   <td>9<br>(69%)</td>
  </tr>
  <tr>
   <td>db-custom-4-26624</td>
   <td>26</td>
   <td>6<br>(23%)</td>
   <td>19<br>(73%)</td>
   <td>19<br>(73%)</td>
  </tr>
  <tr>
   <td>db-custom-8-53248</td>
   <td>52</td>
   <td>11<br>(21%)</td>
   <td>38<br>(73%)</td>
   <td>38<br>(73%)</td>
  </tr>
  <tr>
   <td>db-custom-16-106496</td>
   <td>104</td>
   <td>21<br>(20%)</td>
   <td>75<br>(72%)</td>
   <td>75<br>(72%)</td>
  </tr>
  <tr>
   <td>db-custom-32-212992</td>
   <td>208</td>
   <td>42<br>(20%)</td>
   <td>150<br>(72%)</td>
   <td>150<br>(72%)</td>
  </tr>
  <tr>
   <td>db-custom-64-425984</td>
   <td>416</td>
   <td>84<br>(20%)</td>
   <td>300<br>(72%)</td>
   <td>300<br>(72%)</td>
  </tr>
  <tr>
   <td>db-custom-96-638976</td>
   <td>624</td>
   <td>125<br>(20%)</td>
   <td>450<br>(72%)</td>
   <td>450<br>(72%)</td>
  </tr>
</table>
</dd>
</dl>

<p>If the memory usage is high for your instance and you're
experiencing out-of-memory (OOM) events, then you can enable the
<code translate="no" dir="ltr">innodb_cloudsql_managed_buffer_pool</code> flag to reduce the value
of your <code translate="no" dir="ltr">innodb_buffer_pool_size</code> temporarily.
For more information, see
<a href="/sql/docs/mysql/optimize-high-memory-usage#enable-managed-buffer-pool">Enable managed buffer pool</a>.
(<a href="https://cloud.google.com/products#product-launch-stages">Preview</a>)</p>
When managed buffer pool makes adjustments to value of <code translate="no" dir="ltr">innodb_buffer_pool_size</code>, the
changes aren't reflected in the flag value displayed in Google Cloud console. In order to view the
current value of <code translate="no" dir="ltr">innodb_buffer_pool_size</code> when managed buffer pool is enabled, you
can query the flag value by using the MySQL client:

<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="SQL"><span class="devsite-syntax-k">show</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">global</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-n">variables</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">like</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-s1">'innodb_buffer_pool_size'</span><span class="devsite-syntax-p">;</span></pre></devsite-code>

<dt id="tips-optimized-write">innodb_cloudsql_optimized_write</dt>
<dd>
This flag is available only for Cloud SQL Enterprise Plus edition instances. The default value
is `ON`.

This flag improves write performance by optimizing the flushing algorithm,
controlling flush limits, and adjusting background activity to prioritize
your database write operations. In addition, this flag enables an improved crash
recovery algorithm to reduce crash recovery time and
utilizes unused disk I/O throughput adaptively to accelerate buffer pool warm-up.

For the majority of use cases,
you can experience better performance such as improved throughput and reduced
latency with this flag enabled.
However, if your database write operations cause extremely heavy load
on the server, then the flag can delay some background activities.
This delay can cause a small increase in disk usage, which decreases automatically after the load subsides.

By default, the <code translate="no" dir="ltr">innodb_cloudsql_optimized_write</code> flag is
enabled for all new and upgraded Cloud SQL Enterprise Plus edition instances. For existing Cloud SQL Enterprise Plus edition instances, this flag is enabled after the related maintenance update is applied.

If you need to disable the flag, then run the following command.

<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="Bash">gcloud<span class="devsite-syntax-w"> </span>sql<span class="devsite-syntax-w"> </span>instances<span class="devsite-syntax-w"> </span>patch<span class="devsite-syntax-w"> </span><var translate="no">INSTANCE_NAME</var><span class="devsite-syntax-w"> </span><span class="devsite-syntax-se">\</span>
<span class="devsite-syntax-w"> </span>--database-flags<span class="devsite-syntax-o">=</span><span class="devsite-syntax-s2">"innodb_cloudsql_optimized_write=OFF"</span></pre></devsite-code>

Changing the value of the flag requires restarting the instance.

</dd>

<dt id="tips-file-per-table">innodb_file_per_table</dt>
<dd>
<p>For all MySQL versions 5.6 and higher, the default value is <code translate="no" dir="ltr">ON</code>.
</p>
</dd>

<dt id="tips-flush-log">innodb_flush_log_at_trx_commit, sync_binlog</dt>
<dd>
  For full ACID compliance, and to maintain durability and consistency in a replication setup,
  the <code translate="no" dir="ltr">innodb_flush_log_at_trx_commit</code> and the <code translate="no" dir="ltr">sync_binlog</code>
  flags must be set to the default value of <code translate="no" dir="ltr">1</code>. If you change the default value,
  then durability might decrease, which might lead to
  inconsistency between the primary instance and replicas. Therefore, the instance
  loses its SLA coverage. In addition, any of the following might occur:
  <ul>
      <li>Data loss in certain situations, such as a VM crash or failover for regional HA instance</li>
      <li>Out-of-sync data in binary log and InnoDB data files</li>
      <li>PITR data loss or failure</li>
      <li>Data inconsistency between a primary instance and its replicas</li>
      <li>A replication break</li>
  </ul>

   Setting the value of the
  <code translate="no" dir="ltr">innodb_flush_log_at_trx_commit</code> or <code translate="no" dir="ltr">sync_binlog</code>
  flag to non-default values for primary, standalone, and HA instances causes
  reduced durability.

  If you need higher performance for read replicas, then we recommend setting the
  <code translate="no" dir="ltr">innodb_flush_log_at_trx_commit</code> value to <code translate="no" dir="ltr">2</code>.
  Cloud SQL does not support setting the value for this flag to 0. If you
  set the flag value to 2, you must either disable the binary log on the
  replica, or set
  <a href="/sql/docs/mysql/flags#mysql-s"><code translate="no" dir="ltr">sync_binlog</code></a> to a
  value other than 1 for higher performance.

  Cloud SQL might temporarily change the <code translate="no" dir="ltr">innodb_flush_log_at_trx_commit</code>
  and <code translate="no" dir="ltr">sync_binlog</code> flag values to default when taking a backup. This might
   cause reduced performance when taking backups. To avoid this from impacting
  your instance, you can change
  the backup window when instance usage is low. For more information,
  see <a href="/sql/docs/mysql/backup-recovery/backing-up">Create and manage on-demand and
  automatic backups</a>.
</dd>

<dt id="tips-flush-log-timeout">innodb_flush_log_at_timeout</dt>
<dd>
 <p><code translate="no" dir="ltr">innodb_flush_log_at_timeout</code> lets you modify the frequency
 of page flushes so you can avoid impacting the performance of binary log group
  commit. The default setting is once per second.</p>
 <p>Cloud SQL has extended this flag to support specifying a time period
 in microseconds.</p>
 <p>Examples:
   <ul>
    <li><code translate="no" dir="ltr">0.001</code> to specify 1 ms</li>
    <li><code translate="no" dir="ltr">0.0001</code> to specify 100 us</li>
    <li><code translate="no" dir="ltr">12.5</code> to specify 12.5 seconds</li>
    <li><code translate="no" dir="ltr">12.005</code> to specify 12 seconds and 5 ms</li>
    <li><code translate="no" dir="ltr">0.005100</code> to specify 5 ms and 100 us</li>
  </p>
  </ul>
<p>For certain workloads, using whole second granularity for
  flushing pages might be unacceptable in terms of
  potential transaction loss. Instead, you can flush pages using microsecond
  granularity to maintain performance without significantly
  compromising durability.</p>
 <p>The microsecond time periods for the <code translate="no" dir="ltr">innodb_flush_log_at_timeout</code>
  flag are only applicable when the <code translate="no" dir="ltr">innodb_flush_log_at_trx_commit</code>
  durability flag is set to <code translate="no" dir="ltr">2</code>.</p>
 <p>The flushing of pages might happen more or less frequently than the value
 specified for <code translate="no" dir="ltr">innodb_flush_log_at_timeout</code> and the value is not the
 upper bound.</p>
</dd>

<dt id="tips-redo-logs">innodb_redo_log_capacity, innodb_log_file_size</dt>
<dd>
<p>If you configure a value for the <code translate="no" dir="ltr">innodb_redo_log_capacity</code>
flag,
then Cloud SQL ignores any value that you define for
the <code translate="no" dir="ltr">innodb_log_file_size</code> flag.

If you don't configure any values for
the <code translate="no" dir="ltr">innodb_redo_log_capacity</code> or <code translate="no" dir="ltr">innodb_log_file_size</code>
flags, then Cloud SQL uses
the default value of the <code translate="no" dir="ltr">innodb_redo_log_capacity</code> flag, or <code translate="no" dir="ltr">104857600</code> (100&nbsp;MB).

If you don't configure the <code translate="no" dir="ltr">innodb_redo_log_capacity</code> flag, but configure
the <code translate="no" dir="ltr">innodb_log_file_size</code> flag,
then the value of your innodb redo log size is calculated by <code translate="no" dir="ltr">innodb_log_file_size</code> * <code translate="no" dir="ltr">innodb_log_file_in_group</code>. For example, if you configure
<code translate="no" dir="ltr">innodb_log_file_size</code> to a value of 10&nbsp;GB and the default value of
<code translate="no" dir="ltr">innodb_log_file_in_group</code> is <code translate="no" dir="ltr">2</code>, then the effective
value of your innodb redo log size is 20&nbsp;GB.</p>
</dd>

<dt id="tips-heap">max_heap_table_size, tmp_table_size</dt>
<dd>
<p>Exhausting the available instance memory can occur when you set
<code translate="no" dir="ltr">tmp_table_size</code> and <code translate="no" dir="ltr">max_heap_table_size</code> too high for
the number of concurrent queries the instance processes. Exhausting the memory
results in an instance crash and restart.
</p>

<p>
For more information about working with these flags, see
<a href="https://dev.mysql.com/doc/refman/8.0/en/internal-temporary-tables.html" target="_blank">How MySQL Uses Internal Temporary Tables</a>
  and <a href="https://dev.mysql.com/doc/refman/8.0/en/memory-storage-engine.html" target="_blank">The MEMORY Storage Engine</a>.
</p>
</dd>
  <dt id="tips-performance-schema">performance_schema</dt>
<dd>
<p>You can't enable this flag on instances with a shared core (less than 3 GB of RAM).
  If you enable this flag, then you can't change your machine type to a size
  that does not support the flag; you must first disable this flag.</p>

<p>For MySQL 8.0 and later instances with 12&nbsp;GB to 15&nbsp;GB RAM,
  the default value of the <code translate="no" dir="ltr">performance_schema</code> is <code translate="no" dir="ltr">on</code>;
  however, the default performance schema instruments are
  disabled. For more information about default performance
  schema instruments, see
  <a href="https://dev.mysql.com/doc/refman/8.0/en/performance-schema-setup-instruments-table.html">
  The setup_instruments Table (MySQL 8.0)</a> or
  <a href="https://dev.mysql.com/doc/refman/8.4/en/performance-schema-setup-instruments-table.html">
  The setup_instruments Table (MySQL 8.4)</a>. If you want to enable and use
  performance schema instruments in a 12&nbsp;GB to 15&nbsp;GB
  RAM instance, then you must <b>explicitly</b> set the <code translate="no" dir="ltr">performance_schema</code>
  flag to <code translate="no" dir="ltr">on</code> instead of deferring to the default value.
  For MySQL 8.0 and later instances with &ge;15&nbsp;GB RAM,
  the default value of the <code translate="no" dir="ltr">performance_schema</code> is <code translate="no" dir="ltr">on</code>
  the default performance schema instruments are enabled.</p>

<p></dd>
  <dt id="tips-event-scheduler">event_scheduler</dt>
<dd>
  MySQL Events, also known as scheduled events, are tasks that you can schedule.
  Scheduled events are a group of one or more SQL statements that are set to
  execute at one or more specified intervals. The default value for MySQL 5.7 is
  <code translate="no" dir="ltr">OFF</code> and the default value for MySQL 8.0 is <code translate="no" dir="ltr">ON</code>. To
  learn more about the <code translate="no" dir="ltr">event_scheduler</code> flag, see
  <a href="https://dev.mysql.com/doc/refman/5.7/en/server-system-variables.html#sysvar_event_scheduler">
  event_scheduler</a>.
  If the <code translate="no" dir="ltr">event_scheduler</code> flag is set to <code translate="no" dir="ltr">ON</code> for a read
  replica, it can cause errors based on the type of statements defined in the
  events:
  <ul>
    <li> If your scheduled event is a <code translate="no" dir="ltr">write</code> event on a read
    replica, it causes an error as read replicas are read only. See
    <a href="/sql/docs/mysql/replication#read-replicas">Read Replicas</a> for
    more information. </li>
    <li> If your scheduled event contains a stop operation, such as
    <code translate="no" dir="ltr">kill</code>, <code translate="no" dir="ltr">event_scheduler</code> applies it to the replica.
    This stops the replication  and delete the replica. </li>
  </ul>
  To avoid such errors, set the <code translate="no" dir="ltr">event_scheduler</code> flag to <code translate="no" dir="ltr">OFF</code>
  when creating replicas.</p>

<p>For more information on how to enable or disable <code translate="no" dir="ltr">event_scheduler</code>,
  see <a href="/sql/docs/mysql/flags#config">Configure database flags</a>.
</dd></p>

<p><dt id="tips-skip-errors">replica_skip_errors,slave_skip_errors</dt>
<dd>
Setting the <code translate="no" dir="ltr">replica_skip_errors</code> or the <code translate="no" dir="ltr">slave_skip_errors</code>
flag can cause replication issues. In general,
if an error occurs while executing a statement, the replication is stopped.
Using this flag will cause the error to be skipped and replication to continue,
leading to inconsistency between the primary instance and replica.
This can also make it harder to troubleshoot replication issues.</p>

<p>Cloud SQL recommends only using this flag if necessary. If you
are experiencing replication errors, see <a href="/sql/docs/troubleshooting#replication">
Troubleshooting Cloud SQL: Replication</a> more information on how to resolve
this issue.
</dd></p>

<p><dt id="tips-option-set">
character_set_client<br>
character_set_connection<br>
character_set_results<br>
collation_connection<br>
innodb_buffer_pool_dump_now<br>
innodb_buffer_pool_load_abort<br>
innodb_buffer_pool_load_now<br>
innodb_ft_aux_table<br>
foreign_key_checks<br>
sql_select_limit<br>
unique_checks</dt>
<dd>
These flags can&#39;t be selected directly in the Google Cloud console or using gcloud CLI.
To use these flags, use the following command:</p>

<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="Bash">SET<span class="devsite-syntax-w"> </span>GLOBAL<span class="devsite-syntax-w"> </span><var translate="no"><span class="devsite-syntax-nv">FLAG_NAME</span></var><span class="devsite-syntax-o">=</span><var translate="no">FLAG_VALUE</var></pre></devsite-code>

<p>Using the <code translate="no" dir="ltr">SET GLOBAL</code> command requires the <code translate="no" dir="ltr">CLOUDSQL_SPECIAL_SYSEM_VARIABLES_ADMIN</code>
privilege, which is granted to the <code translate="no" dir="ltr">cloudsqlsuperuser</code> role.</p>
<aside class="note"><strong>Note:</strong><span> The <code translate="no" dir="ltr">CLOUDSQL_SPECIAL_SYSEM_VARIABLES_ADMIN</code> privilege
is only available in MySQL 8.0 for Cloud SQL.</span></aside>
<p>For more information on how to grant special privilege access to a specific user, see
<a href="/sql/docs/mysql/users#cloudsqlsuperuser">About MySQL users</a>.
These flags are non-persisted. When your Cloud SQL instance is recreated or
restarted, the flag settings are reset back to default value.</dd>
</dl></p>

<dl>
      <dt id="tips-binlog-order">binlog_order_commits</dt>
        <dd>
            <p>
            The default value for the <code translate="no" dir="ltr">binlog_order_commits</code> flag is
            <code translate="no" dir="ltr">ON</code>. Cloud SQL recommends to not change the default
            value of this flag. If the default value is changed to
            <code translate="no" dir="ltr">OFF</code>, transactions in the same binary log group will
            commit in a different order than when they were written in the binary
            log. This impacts the following operations that execute transactions
            in the binary log order:
                  <ul>
                        <li><b>Replication</b>: may lead to data inconsistency
                        between the source and replicas
                        <li><b>Point-in-time-recovery</b>: may lead to data
                        inconsistency between the PITR restored state and historical state
                  <ul>
            </p>
        </dd>
</dl>

<dl>
<dt id="tips-multi-value">optimizer_switch,optimizer_trace,optimizer_trace_features</dt>
<dd>
<p>
Optimizer flags have comma-separated values. You can set these flags using
the Console or gcloud. For more information on how to set this flag using the
console, see <a href="/sql/docs/mysql/flags#set_a_database_flag">Configure database flags</a>.
If using gcloud, you can specify the value for these flags using two different ways:
<ul>
<li><a href="/sdk/gcloud/reference/topic/flags-file">gcloud topic flags-file</a></li>
<li><a href="/sdk/gcloud/reference/topic/escaping">gcloud topic escaping-file</a></li>
</ul>

To set multiple optimizer sub-flags in one command, use the comma delimiter to separate
each flag name. If you set a single sub-flag value using the
gcloud command, it overwrites all previously set sub-flags.

For example, if you run the following command, the expected value for
the <code translate="no" dir="ltr">batched_key_access</code> sub-flag is set to <code translate="no" dir="ltr">on</code> and all
other sub-flags for optimizer_flags are set to their default values.

<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="Bash">gcloud<span class="devsite-syntax-w"> </span>sql<span class="devsite-syntax-w"> </span>instances<span class="devsite-syntax-w"> </span>patch<span class="devsite-syntax-w"> </span>my-instance<span class="devsite-syntax-w"> </span>--database-flags<span class="devsite-syntax-o">=</span>^~^optimizer_switch<span class="devsite-syntax-o">=</span><span class="devsite-syntax-nv">batched_key_access</span><span class="devsite-syntax-o">=</span>on</pre></devsite-code>

If you run the following command, the value of the
<code translate="no" dir="ltr">block_nested_loop</code> sub-flag is set to <code translate="no" dir="ltr">on</code> and all
other sub-flags for optimizer_switch are overwritten and set to their default values.

<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="Bash">gcloud<span class="devsite-syntax-w"> </span>sql<span class="devsite-syntax-w"> </span>instances<span class="devsite-syntax-w"> </span>patch<span class="devsite-syntax-w"> </span>my-instance<span class="devsite-syntax-w"> </span>--database-flags<span class="devsite-syntax-o">=</span>^~^optimizer_switch<span class="devsite-syntax-o">=</span><span class="devsite-syntax-nv">block_nested_loop</span><span class="devsite-syntax-o">=</span>on</pre></devsite-code>

This includes <code translate="no" dir="ltr">batched_key_access</code>, which was set to <code translate="no" dir="ltr">on</code>
by the previous command. To keep all previously set sub-flags and add new
ones, you must add the values of all sub-flags you want to set when adding a
new sub-flag.
</p>
</dd>
</dl>

<h2 id="mysql-flag-changes" data-text="System flags changed in Cloud SQL" tabindex="-1">System flags changed in Cloud SQL</h2>

<p>All other database system flags that are not listed in the
<a href="#list-flags-mysql">supported flags</a> section are called managed flags. For
certain managed flags, Cloud SQL sets the flag to a value other than the
default setting to ensure Cloud SQL instances run reliably. You can&#39;t change
the values on these system flags.</p>

<p>Managed flags with a non-default setting are listed below.</p>

<table>
  <tr>
    <th>Variable Name</th>
    <th>Setting in Cloud SQL.</th>
    <th>Notes</th>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/5.6/en/replication-options-binary-log.html#sysvar_binlog_format">binlog_format</a></td>
    <td>ROW</td>
    <td>Differs in MySQL 5.6 only</td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/5.6/en/replication-options-binary-log.html#sysvar_binlog_error_action">binlog_error_action</a></td>
    <td>ABORT_SERVER</td>
    <td>Differs in MySQL 5.6 only</td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/innodb-parameters.html#sysvar_innodb_doublewrite_pages">innodb_doublewrite_pages</a></td>
    <td>64</td>
    <td>Applies to MySQL 8.0.26 and above</td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/5.6/en/innodb-parameters.html#sysvar_innodb_file_format">innodb_file_format</a></td>
    <td>Barracuda</td>
    <td>Differs in MySQL 5.6 only</td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-options.html#option_mysqld_memlock">memlock</a></td>
    <td>true</td>
    <td></td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_skip_name_resolve">skip_name_resolve</a></td>
    <td>ON</td>
    <td></td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_relay_log_info_repository">relay_log_info_repository</a></td>
    <td>TABLE</td>
    <td>Removed in MySQL 8.4</td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_relay_log_recovery">relay_log_recovery</a></td>
    <td>ON</td>
    <td></td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-replica.html#sysvar_master_info_repository">master_info_repository</a></td>
    <td>TABLE</td>
    <td>Removed in MySQL 8.4</td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-source.html#sysvar_rpl_semi_sync_master_enabled">rpl_semi_sync_master_enabled</a></td>
    <td>1</td>
    <td></td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/replication-options-source.html#sysvar_rpl_semi_sync_master_timeout">rpl_semi_sync_master_timeout</a></td>
    <td>3000</td>
    <td></td>
  </tr>
  <tr>
    <td><a href="">admin_address</a></td>
    <td>127.0.0.1</td>
    <td>Differs in MySQL 8.0 and later.</td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html#sysvar_create_admin_listener_thread">create_admin_listener_thread</a></td>
    <td>ON</td>
    <td></td>
  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/server-options.html#option_mysqld_port-open-timeout">port-open-timeout</a></td>
    <td>120</td>
    <td>Differs in MySQL 8.0 and later.</td>  </tr>
  <tr>
    <td><a href="https://dev.mysql.com/doc/refman/8.0/en/partial-revokes.html">partial_revokes</a></td>
    <td>ON</td>
    <td>MySQL 8.0 and later. For more information about this flag, see
    <a href="#partial-revokes">Partial revokes system flag in MySQL 8.0</a>.</td>
  </tr>
</table>

<h3 id="partial-revokes" data-text="partial_revokes system flag in MySQL 8.0 and later" tabindex="-1">partial_revokes system flag in MySQL 8.0 and later</h3>

<p>The <code translate="no" dir="ltr">partial_revokes</code> flag allows you to limit user access on a databases schema.
In Cloud SQL for MySQL version 8.0 and later, the <code translate="no" dir="ltr">partial_revokes</code> flag is set to
<code translate="no" dir="ltr">ON</code>. This limits the use of wildcard characters when granting or
revoking user privileges to database schemas in MySQL 8.0. Update your
<code translate="no" dir="ltr">GRANT</code> statement to use the full name of the database schema
instead of using wildcard characters.</p>

<p>For example, if you use the following command with the <code translate="no" dir="ltr">%\</code> wildcard character
to grant privileges to a user in MySQL 5.7, then the user will be granted
privileges to all databases ending with <code translate="no" dir="ltr">_foobar</code>.</p>
<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="SQL"><code translate="no" dir="ltr"><span class="devsite-syntax-k">GRANT</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">ALL</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">PRIVILEGES</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">ON</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-o">`%</span><span class="devsite-syntax-err">\</span><span class="devsite-syntax-n">_foobar</span><span class="devsite-syntax-o">`</span><span class="devsite-syntax-p">.</span><span class="devsite-syntax-o">*</span><span class="devsite-syntax-w">  </span><span class="devsite-syntax-k">TO</span><span class="devsite-syntax-w">  </span><span class="devsite-syntax-s1">'testuser'</span><span class="devsite-syntax-o">@</span><span class="devsite-syntax-s1">'%'</span><span class="devsite-syntax-p">;</span>
</code></pre></devsite-code>
<p>However, in MySQL 8.0, users will only be granted access to the database that
is an exact match to <code translate="no" dir="ltr">%\_foobar</code>.</p>

<p>There are two different ways to grant access to multiple databases in MySQL 8.0 and later.</p>

<ol>
<li><p>You can grant permissions to specific databases using the full
database names as shown in the command below:</p>
<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="SQL"><code translate="no" dir="ltr"><span class="devsite-syntax-w">  </span><span class="devsite-syntax-k">grant</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">select</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">on</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-n">test1_foobar</span><span class="devsite-syntax-p">.</span><span class="devsite-syntax-o">*</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">to</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-s1">'testuser'</span><span class="devsite-syntax-o">@</span><span class="devsite-syntax-s1">'%'</span><span class="devsite-syntax-p">;</span>
<span class="devsite-syntax-w">  </span><span class="devsite-syntax-k">grant</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">select</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">on</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-n">test2_foobar</span><span class="devsite-syntax-p">.</span><span class="devsite-syntax-o">*</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">to</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-s1">'testuser'</span><span class="devsite-syntax-o">@</span><span class="devsite-syntax-s1">'%'</span><span class="devsite-syntax-p">;</span>
<span class="devsite-syntax-w">  </span><span class="devsite-syntax-k">grant</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">select</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">on</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-n">test3_foobar</span><span class="devsite-syntax-p">.</span><span class="devsite-syntax-o">*</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">to</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-s1">'testuser'</span><span class="devsite-syntax-o">@</span><span class="devsite-syntax-s1">'%'</span><span class="devsite-syntax-p">;</span>
</code></pre></devsite-code></li>
<li><p>With <code translate="no" dir="ltr">partial_revokes</code>, you can use the <code translate="no" dir="ltr">grant</code> and
<code translate="no" dir="ltr">revoke</code> command to grant user privileges on all database schemas while
 restricting access to a few database schemas.</p>
<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="SQL"><code translate="no" dir="ltr"><span class="devsite-syntax-w">  </span><span class="devsite-syntax-k">grant</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">select</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">on</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-o">*</span><span class="devsite-syntax-p">.</span><span class="devsite-syntax-o">*</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">to</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-s1">'testuser'</span><span class="devsite-syntax-o">@</span><span class="devsite-syntax-s1">'%'</span><span class="devsite-syntax-p">;</span>
<span class="devsite-syntax-w">  </span><span class="devsite-syntax-k">revoke</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">select</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">on</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-n">test3_foobar</span><span class="devsite-syntax-p">.</span><span class="devsite-syntax-o">*</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-k">from</span><span class="devsite-syntax-w"> </span><span class="devsite-syntax-s1">'testuser'</span><span class="devsite-syntax-o">@</span><span class="devsite-syntax-s1">'%'</span><span class="devsite-syntax-p">;</span>
</code></pre></devsite-code>
<p>This grants access to all database schemas while restricting access to
  <code translate="no" dir="ltr">test3_foobar.*</code>.</p></li>
</ol>

<h2 id="replication-filters" data-text="Replication filters" tabindex="-1">Replication filters</h2>

<p>Replication filters can be set only on Cloud SQL replicas. Each replication
filter is set as a single flag for multiple databases where each database name
is separate by a comma. You can set up a replication filter on a Cloud SQL replica using
console or the following command:</p>

<div></div><devsite-code><pre class="devsite-click-to-copy" translate="no" dir="ltr" is-upgraded syntax="Bash">gcloud<span class="devsite-syntax-w"> </span>sql<span class="devsite-syntax-w"> </span>instances<span class="devsite-syntax-w"> </span>patch<span class="devsite-syntax-w"> </span><var translate="no">REPLICA_NAME</var><span class="devsite-syntax-w"> </span>--database-flags<span class="devsite-syntax-o">=</span>^~^<var translate="no">REPLICATION_FILTER_NAME</var><span class="devsite-syntax-o">=</span><var translate="no">DATABASE_NAME1</var>,<var translate="no">DATABASE_NAME</var>,<span class="devsite-syntax-w"> </span>etc</pre></devsite-code>

<p>Replication filters don&#39;t support database names that contain comma values. The
<code translate="no" dir="ltr">^~^</code> value in the preceding command is necessary for database flags that are
comma-separated values.</p>

<p>When you set a replication filter flag, keep the following in mind:
<ul>
      <li>If the replica becomes unhealthy, then data filtered by replication filters
      can appear on the replica as Cloud SQL uses source data from the primary to
      rebuild the instance replica.</li>
      <li>You can&#39;t set replication filters on the <code translate="no" dir="ltr">mysql</code> schema.</li>
      <li>Replication filter rules don&#39;t apply to serverless exports.</li>
</ul></p>

<h2 id="index-advisor-flags" data-text="Index advisor flags" tabindex="-1">Index advisor flags</h2>

<p>The following is a list of database flags that Cloud SQL for MySQL uses
to enable and manage features specific to the
<a href="/sql/docs/mysql/index-advisor-overview">index advisor</a>.</p>

<table>
  <tr>
      <th>Flag name</th>
      <th>Type<br>Acceptable values and notes</th>
      <th>Restart<br>Required?</th>
  </tr>
  <tr>
      <td>cloudsql_index_advisor_auto_advisor_schedule</td>
      <td>
            <code translate="no" dir="ltr">string</code>
            <br> default: <code translate="no" dir="ltr">00:00</code>
      </td>
      <td>No</td>
  </tr>
  <tr>
      <td>cloudsql_index_advisor_run_at_timestamp</td>
      <td>
            <code translate="no" dir="ltr">Datetime</code>
            <br> default: <code translate="no" dir="ltr">00:00:00</code>
      </td>
      <td>No</td>
  </tr>
</table>

<h2 id="aliased-flags" data-text="Aliased flags" tabindex="-1">Aliased flags</h2>

<p>The following list below contains the flag names that have been changed by Cloud SQL
for MySQL versions 8.0.26 and above.</p>

<table>
  <tr><th>Deprecated flag name</th>
      <th>New flag name</th>
  </tr>
  <tr>
      <td>log_slow_slave_statements</td>
      <td>log_slow_replica_statements</td>
  </tr>
    <tr>
      <td>master_verify_checksum</td>
      <td>source_verify_checksum</td>
  </tr>
    <tr>
      <td>slave_checkpoint_group</td>
      <td>replica_checkpoint_group</td>
  </tr>
    <tr>
      <td>slave_checkpoint_period</td>
      <td>replica_checkpoint_period</td>
  </tr>
    <tr>
      <td>slave_compressed_protocol</td>
      <td>replica_compressed_protocol</td>
  </tr>
    <tr>
      <td>slave_net_timeout</td>
      <td>replica_net_timeout</td>
  </tr>
    <tr>
      <td>slave_parallel_type</td>
      <td>replica_parallel_type</td>
  </tr>
    <tr>
      <td>slave_parallel_workers</td>
      <td>replica_parallel_workers</td>
  </tr>
    <tr>
      <td>slave_pending_jobs_size_max</td>
      <td>replica_pending_jobs_size_max</td>
  </tr>
    <tr>
      <td>slave_preserve_commit_order</td>
      <td>replica_preserve_commit_order</td>
  </tr>
<tr>
      <td>slave_skip_errors</td>
      <td>replica_skip_errors</td>
  </tr>
  <tr>
      <td>slave_sql_verify_checksum</td>
      <td>replica_sql_verify_checksum</td>
  </tr>
    <tr>
      <td>slave_transaction_retries</td>
      <td>replica_transaction_retries</td>
  </tr>
    <tr>
      <td>slave_type_conversions</td>
      <td>replica_type_conversions</td>
  </tr>
    <tr>
      <td>sync_master_info</td>
      <td>sync_source_info</td>
  </tr>
</table>

<p>If your Cloud SQL instance is using a deprecated flag name, then
edit your Cloud SQL instance, delete the deprecated flag name, and add the
new flag to your instance. For more information, see
<a href="/sql/docs/mysql/flags#set_a_database_flag">Setup a database flag</a>.</p>

<h2 id="troubleshooting-flags" data-text="Troubleshooting" tabindex="-1">Troubleshooting</h2>

<table>
  <thead>
    <th width="30%">Issue</th>
    <th width="70%">Troubleshooting</th>
  </thead>
  <tbody>
  
  <tr>
    <td>After enabling a flag the instance loops between panicking and crashing.</td>
    <td>Contact <a href="/sql/docs/getting-support"> customer support</a> to
    request  flag removal followed by a <code translate="no" dir="ltr">hard drain</code>. This forces the
    instance to restart on a different host with a fresh configuration without
    the undesired flag or setting.
    </td>
  </tr>
  <tr>
    <td>You see the error message <code translate="no" dir="ltr">Bad syntax for dict arg</code> when
    trying to set a flag.
    <td><a href="https://cloud.google.com/sdk/gcloud/reference/topic/flags-file">
    Complex parameter values</a>, such as comma-separated lists, require special
    treatment when used with gcloud commands.
    </td>
  </tr>
  
  
  
  </tbody>
</table>

<h2 id="whats_next" data-text="What's next" tabindex="-1">What's next</h2>

<ul>
<li>Learn more about <a href="https://dev.mysql.com/doc/refman/8.0/en/server-system-variables.html" target="_blank">MySQL system variables</a>.</li>
<li>Learn more about <a href="/sql/docs/mysql/operational-guidelines">Operational Guidelines</a>.</li>
</ul>
  

  
    <devsite-hats-survey class="nocontent" data-nosnippet
      hats-id="mwETRvWii0eU5NUYprb0Y9z5GVbc"
      listnr-id="83405"></devsite-hats-survey>
  

  
</div>

  
    
    
      
    <devsite-thumb-rating position="footer">
    </devsite-thumb-rating>
  
       
         <devsite-feedback
  position="footer"
  project-name="Cloud SQL for MySQL"
  product-id="82040"
  bucket="documentation"
  context=""
  version="t-devsite-webserver-20260428-r00-rc01.477318040238770238"
  data-label="Send Feedback Button"
  track-type="feedback"
  track-name="sendFeedbackLink"
  track-metadata-position="footer"
  class="nocontent"
  data-nosnippet
  
  
    project-feedback-url="https://issuetracker.google.com/issues/new?component=187202"
  
  
    
      project-icon="https://docs.cloud.google.com/_static/clouddocs/images/icons/products/sql-color.svg"
    
  
  
    project-support-url="https://cloud.google.com/sql/docs/mysql/getting-support"
  
  
  >

  <button>
  
    
    Send feedback
  
  </button>
</devsite-feedback>
       
    
    
  

  <div class="devsite-floating-action-buttons"></div></article>


<devsite-content-footer class="nocontent" data-nosnippet>
  <p>Except as otherwise noted, the content of this page is licensed under the <a href="https://creativecommons.org/licenses/by/4.0/">Creative Commons Attribution 4.0 License</a>, and code samples are licensed under the <a href="https://www.apache.org/licenses/LICENSE-2.0">Apache 2.0 License</a>. For details, see the <a href="https://developers.google.com/site-policies">Google Developers Site Policies</a>. Java is a registered trademark of Oracle and/or its affiliates.</p>
  <p>Last updated 2026-05-08 UTC.</p>
</devsite-content-footer>


<devsite-notification
>
</devsite-notification>


  
<div class="devsite-content-data">
  
    
    
    <template class="devsite-thumb-rating-feedback">
      <devsite-feedback
  position="thumb-rating"
  project-name="Cloud SQL for MySQL"
  product-id="82040"
  bucket="documentation"
  context=""
  version="t-devsite-webserver-20260428-r00-rc01.477318040238770238"
  data-label="Send Feedback Button"
  track-type="feedback"
  track-name="sendFeedbackLink"
  track-metadata-position="thumb-rating"
  class="nocontent"
  data-nosnippet
  
  
    project-feedback-url="https://issuetracker.google.com/issues/new?component=187202"
  
  
    
      project-icon="https://docs.cloud.google.com/_static/clouddocs/images/icons/products/sql-color.svg"
    
  
  
    project-support-url="https://cloud.google.com/sql/docs/mysql/getting-support"
  
  
  >

  <button>
  
    Need to tell us more?
  
  </button>
</devsite-feedback>
    </template>
  
  
    <template class="devsite-content-data-template">
      [[["Easy to understand","easyToUnderstand","thumb-up"],["Solved my problem","solvedMyProblem","thumb-up"],["Other","otherUp","thumb-up"]],[["Hard to understand","hardToUnderstand","thumb-down"],["Incorrect information or sample code","incorrectInformationOrSampleCode","thumb-down"],["Missing the information/samples I need","missingTheInformationSamplesINeed","thumb-down"],["Other","otherDown","thumb-down"]],["Last updated 2026-05-08 UTC."],[],[]]
    </template>
  
</div>
            
          </devsite-content>
        </main>
        <devsite-footer-promos class="devsite-footer">
          
            
          
        </devsite-footer-promos>
        <devsite-footer-linkboxes class="devsite-footer">
          
            
<nav class="devsite-footer-linkboxes nocontent"
     aria-label="Footer links"
     data-nosnippet>
  
  <ul class="devsite-footer-linkboxes-list">
    
    <li class="devsite-footer-linkbox ">
    <h3 class="devsite-footer-linkbox-heading no-link">Products and pricing</h3>
      <ul class="devsite-footer-linkbox-list">
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//cloud.google.com/products/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 1)"
            track-name="see all products"track-type="footer link"track-metadata-child_headline="products and pricing"track-metadata-eventDetail="cloud.google.com/products/"track-metadata-position="footer"track-metadata-module="footer">
            
          
            See all products
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//cloud.google.com/pricing/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 2)"
            track-name="google cloud pricing"track-type="footer link"track-metadata-child_headline="products and pricing"track-metadata-eventDetail="cloud.google.com/pricing/"track-metadata-position="footer"track-metadata-module="footer">
            
          
            Google Cloud pricing
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//cloud.google.com/marketplace/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 3)"
            track-type="footer link"track-metadata-child_headline="resources"track-name="google cloud marketplace"track-metadata-module="footer"track-metadata-eventDetail="cloud.google.com/marketplace/"track-metadata-position="footer">
            
          
            Google Cloud Marketplace
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//cloud.google.com/contact/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 4)"
            track-metadata-eventDetail="cloud.google.com/contact/"track-metadata-position="footer"track-metadata-module="footer"track-name="contact sales"track-type="footer link"track-metadata-child_headline="engage">
            
              
              
            
          
            Contact sales
          
          </a>
          
          
        </li>
        
      </ul>
    </li>
    
    <li class="devsite-footer-linkbox ">
    <h3 class="devsite-footer-linkbox-heading no-link">Support</h3>
      <ul class="devsite-footer-linkbox-list">
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//discuss.google.dev/c/google-cloud/14/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 1)"
            target="_blank"track-type="footer link"track-metadata-child_headline="engage"rel="noopener"track-name="google cloud community"track-metadata-module="footer"track-metadata-eventDetail="www.googlecloudcommunity.com"track-metadata-position="footer">
            
          
            Community forums
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//cloud.google.com/support-hub/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 2)"
            track-metadata-eventDetail="cloud.google.com/support-hub/"track-metadata-position="footer"track-metadata-module="footer"track-name="support"track-type="footer link"track-metadata-child_headline="resources">
            
          
            Support
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//docs.cloud.google.com/release-notes"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 3)"
            track-type="footer link"track-metadata-child_headline="resources"track-name="release notes"track-metadata-module="footer"track-metadata-eventDetail="cloud.google.com/release-notes/"track-metadata-position="footer">
            
          
            Release Notes
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//status.cloud.google.com"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 4)"
            track-metadata-eventDetail="status.cloud.google.com"track-metadata-position="footer"track-metadata-module="footer"track-name="system status"track-type="footer link"target="_blank"track-metadata-child_headline="resources">
            
              
              
            
          
            System status
          
          </a>
          
          
        </li>
        
      </ul>
    </li>
    
    <li class="devsite-footer-linkbox ">
    <h3 class="devsite-footer-linkbox-heading no-link">Resources</h3>
      <ul class="devsite-footer-linkbox-list">
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//github.com/googlecloudPlatform/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 1)"
            track-type="footer link"track-metadata-child_headline="resources"track-name="github"track-metadata-module="footer"track-metadata-eventDetail="github.com/googlecloudPlatform/"track-metadata-position="footer">
            
          
            GitHub
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="/docs/get-started/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 2)"
            track-metadata-position="footer"track-metadata-eventDetail="cloud.google.com/docs/get-started/"track-metadata-module="footer"track-name="google cloud quickstarts"track-metadata-child_headline="resources"track-type="footer link">
            
          
            Getting Started with Google Cloud
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="/docs/samples"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 3)"
            track-metadata-position="footer"track-metadata-eventDetail="cloud.google.com/docs/samples"track-metadata-module="footer"track-name="code samples"track-metadata-child_headline="resources"track-type="footer link">
            
          
            Code samples
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="/architecture/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 4)"
            track-metadata-child_headline="resources"track-type="footer link"track-name="cloud architecture center"track-metadata-module="footer"track-metadata-position="footer"track-metadata-eventDetail="cloud.google.com/architecture/">
            
          
            Cloud Architecture Center
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//cloud.google.com/learn/training/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 5)"
            track-name="training"track-type="footer link"track-metadata-child_headline="resources"track-metadata-eventDetail="cloud.google.com/learn/training/"track-metadata-position="footer"track-metadata-module="footer">
            
              
              
            
          
            Training and Certification
          
          </a>
          
          
        </li>
        
      </ul>
    </li>
    
    <li class="devsite-footer-linkbox ">
    <h3 class="devsite-footer-linkbox-heading no-link">Engage</h3>
      <ul class="devsite-footer-linkbox-list">
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//cloud.google.com/blog/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 1)"
            track-metadata-position="footer"track-metadata-eventDetail="cloud.google.com/blog/"track-metadata-module="footer"track-name="blog"track-metadata-child_headline="engage"track-type="footer link">
            
          
            Blog
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//cloud.google.com/events/"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 2)"
            track-name="events"track-metadata-child_headline="engage"track-type="footer link"track-metadata-position="footer"track-metadata-eventDetail="cloud.google.com/events/"track-metadata-module="footer">
            
          
            Events
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//x.com/googlecloud"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 3)"
            track-metadata-module="footer"track-metadata-position="footer"track-metadata-eventDetail="x.com/googlecloud"track-metadata-child_headline="engage"rel="noopener"target="_blank"track-type="footer link"track-name="follow on x">
            
          
            X (Twitter)
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//www.youtube.com/googlecloud"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 4)"
            track-metadata-position="footer"track-metadata-eventDetail="www.youtube.com/googlecloud"track-metadata-module="footer"track-name="google cloud on youtube"rel="noopener"track-metadata-child_headline="engage"track-type="footer link"target="_blank">
            
          
            Google Cloud on YouTube
          
          </a>
          
          
        </li>
        
        <li class="devsite-footer-linkbox-item">
          
          <a href="//www.youtube.com/googlecloudplatform"
             class="devsite-footer-linkbox-link gc-analytics-event"
             data-category="Site-Wide Custom Events"
            
             data-label="Footer Link (index 5)"
            rel="noopener"track-metadata-child_headline="engage"target="_blank"track-type="footer link"track-name="google cloud tech on youtube"track-metadata-module="footer"track-metadata-position="footer"track-metadata-eventDetail="www.youtube.com/googlecloudplatform">
            
              
              
            
          
            Google Cloud Tech on YouTube
          
          </a>
          
          
        </li>
        
      </ul>
    </li>
    
  </ul>
  
</nav>
          
        </devsite-footer-linkboxes>
        <devsite-footer-utility class="devsite-footer">
          
            

<div class="devsite-footer-utility nocontent" data-nosnippet>
  

  
  <nav class="devsite-footer-utility-links" aria-label="Utility links">
    
    <ul class="devsite-footer-utility-list">
      
      <li class="devsite-footer-utility-item
                 ">
        
        
        <a class="devsite-footer-utility-link gc-analytics-event"
           href="//about.google/"
           data-category="Site-Wide Custom Events"
           data-label="Footer About Google link"
         
           track-metadata-module="utility footer"
         
           track-metadata-position="footer"
         
           track-metadata-eventDetail="//about.google/"
         
           track-type="footer link"
         
           target="_blank"
         
           track-name="about google"
         >
          About Google
        </a>
        
      </li>
      
      <li class="devsite-footer-utility-item
                 devsite-footer-privacy-link">
        
        
        <a class="devsite-footer-utility-link gc-analytics-event"
           href="//policies.google.com/privacy"
           data-category="Site-Wide Custom Events"
           data-label="Footer Privacy link"
         
           track-metadata-module="utility footer"
         
           track-metadata-eventDetail="//policies.google.com/privacy"
         
           track-metadata-position="footer"
         
           target="_blank"
         
           track-type="footer link"
         
           track-name="privacy"
         >
          Privacy
        </a>
        
      </li>
      
      <li class="devsite-footer-utility-item
                 ">
        
        
        <a class="devsite-footer-utility-link gc-analytics-event"
           href="//policies.google.com/terms?hl=en"
           data-category="Site-Wide Custom Events"
           data-label="Footer Site terms link"
         
           track-type="footer link"
         
           target="_blank"
         
           track-name="site terms"
         
           track-metadata-module="utility footer"
         
           track-metadata-position="footer"
         
           track-metadata-eventDetail="//www.google.com/intl/en/policies/terms/regional.html"
         >
          Site terms
        </a>
        
      </li>
      
      <li class="devsite-footer-utility-item
                 ">
        
        
        <a class="devsite-footer-utility-link gc-analytics-event"
           href="//cloud.google.com/product-terms"
           data-category="Site-Wide Custom Events"
           data-label="Footer Google Cloud terms link"
         
           track-name="google cloud terms"
         
           track-type="footer link"
         
           track-metadata-position="footer"
         
           track-metadata-eventDetail="//cloud.google.com/product-terms"
         
           track-metadata-module="utility footer"
         >
          Google Cloud terms
        </a>
        
      </li>
      
      <li class="devsite-footer-utility-item
                 glue-cookie-notification-bar-control">
        
        
        <a class="devsite-footer-utility-link gc-analytics-event"
           href="#"
           data-category="Site-Wide Custom Events"
           data-label="Footer Manage cookies link"
         
           track-metadata-module="utility footer"
         
           track-metadata-position="footer"
         
           track-metadata-eventDetail="#"
         
           track-type="footer link"
         
           track-name="Manage cookies"
         
           aria-hidden="true"
         >
          Manage cookies
        </a>
        
      </li>
      
      <li class="devsite-footer-utility-item
                 devsite-footer-carbon-button">
        
        
        <a class="devsite-footer-utility-link gc-analytics-event"
           href="//cloud.google.com/sustainability"
           data-category="Site-Wide Custom Events"
           data-label="Footer Our third decade of climate action: join us link"
         
           track-type="footer link"
         
           track-name="Our third decade of climate action: join us"
         
           track-metadata-module="utility footer"
         
           track-metadata-position="footer"
         
           track-metadata-eventDetail="/sustainability/"
         >
          Our third decade of climate action: join us
        </a>
        
      </li>
      
      <li class="devsite-footer-utility-item
                 devsite-footer-utility-button">
        
        <span class="devsite-footer-utility-description">Sign up for the Google Cloud newsletter</span>
        
        
        <a class="devsite-footer-utility-link gc-analytics-event"
           href="//cloud.google.com/newsletter/"
           data-category="Site-Wide Custom Events"
           data-label="Footer Subscribe link"
         
           track-metadata-position="footer"
         
           track-metadata-eventDetail="/newsletter/"
         
           track-metadata-module="utility footer"
         
           track-name="subscribe"
         
           track-type="footer link"
         >
          Subscribe
        </a>
        
      </li>
      
    </ul>
    
    
<devsite-language-selector>
  <ul role="presentation">
    
    
    <li role="presentation">
      <a role="menuitem" lang="en"
        >English</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="de"
        >Deutsch</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="es"
        >Español</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="es_419"
        >Español – América Latina</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="fr"
        >Français</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="id"
        >Indonesia</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="it"
        >Italiano</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="pt"
        >Português</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="pt_br"
        >Português – Brasil</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="he"
        >עברית</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="zh_cn"
        >中文 – 简体</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="zh_tw"
        >中文 – 繁體</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="ja"
        >日本語</a>
    </li>
    
    <li role="presentation">
      <a role="menuitem" lang="ko"
        >한국어</a>
    </li>
    
  </ul>
</devsite-language-selector>

  </nav>
</div>
          
        </devsite-footer-utility>
        <devsite-panel>
          
        </devsite-panel>
        
      </section>
      </section>
    <devsite-sitemask></devsite-sitemask>
    <devsite-snackbar></devsite-snackbar>
    <devsite-tooltip ></devsite-tooltip>
    <devsite-heading-link></devsite-heading-link>
    <devsite-analytics>
      
        <script type="application/json" analytics>[]</script>
<script type="application/json" tag-management>{&#34;at&#34;: &#34;True&#34;, &#34;ga4&#34;: [], &#34;ga4p&#34;: [], &#34;gtm&#34;: [{&#34;id&#34;: &#34;GTM-5CVQBG&#34;, &#34;purpose&#34;: 1}], &#34;parameters&#34;: {&#34;internalUser&#34;: &#34;False&#34;, &#34;language&#34;: {&#34;machineTranslated&#34;: &#34;False&#34;, &#34;requested&#34;: &#34;en&#34;, &#34;served&#34;: &#34;en&#34;}, &#34;pageType&#34;: &#34;article&#34;, &#34;projectName&#34;: &#34;Cloud SQL for MySQL&#34;, &#34;signedIn&#34;: &#34;False&#34;, &#34;tenant&#34;: &#34;clouddocs&#34;, &#34;recommendations&#34;: {&#34;sourcePage&#34;: &#34;&#34;, &#34;sourceType&#34;: 0, &#34;sourceRank&#34;: 0, &#34;sourceIdenticalDescriptions&#34;: 0, &#34;sourceTitleWords&#34;: 0, &#34;sourceDescriptionWords&#34;: 0, &#34;experiment&#34;: &#34;&#34;}, &#34;experiment&#34;: {&#34;ids&#34;: &#34;&#34;}}}</script>
      
    </devsite-analytics>
    
      <devsite-badger></devsite-badger>
    
    
    <cloudx-user></cloudx-user>



  <cloudx-free-trial-eligible-store freeTrialEligible="true"></cloudx-free-trial-eligible-store>

    
<script nonce="4ZSktYWcPTH7KYvI4agBDRvfyePtHB">
  
  (function(d,e,v,s,i,t,E){d['GoogleDevelopersObject']=i;
    t=e.createElement(v);t.async=1;t.src=s;E=e.getElementsByTagName(v)[0];
    E.parentNode.insertBefore(t,E);})(window, document, 'script',
    'https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs/js/app_loader.js', '[39,"en",null,"/js/devsite_app_module.js","https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e","https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs","https://clouddocs-dot-devsite-v2-prod.appspot.com",null,null,["/_pwa/clouddocs/manifest.json","https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/images/video-placeholder.svg","https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs/images/favicons/onecloud/favicon.ico","https://www.gstatic.com/devrel-devsite/prod/v1b953b434e2033f0160fd97c99360ee5d4d0a613449c2a694360a72d378b9d8e/clouddocs/images/lockup.svg","https://fonts.googleapis.com/css?family=Google+Sans:400,500|Roboto:400,400italic,500,500italic,700,700italic|Roboto+Mono:400,500,700&display=swap"],1,null,[1,6,8,12,14,17,21,25,50,52,63,70,75,76,80,87,91,92,93,97,98,100,101,102,103,104,105,107,108,109,110,112,113,116,117,118,120,122,124,125,126,127,129,130,131,132,133,134,135,136,138,140,141,147,148,149,151,152,156,157,158,159,161,163,164,168,169,170,179,180,182,183,186,191,193,196],"AIzaSyAP-jjEJBzmIyKR4F-3XITp8yM9T1gEEI8","AIzaSyB6xiKGDR5O3Ak2okS4rLkauxGUG7XP0hg","docs.cloud.google.com","AIzaSyAQk0fBONSGUqCNznf6Krs82Ap1-NV6J4o","AIzaSyCCxcqdrZ_7QMeLCRY20bh_SXdAYqy70KY",null,null,null,["Search__enable_dynamic_content_confidential_banner","Profiles__enable_completecodelab_endpoint","Profiles__enable_dashboard_curated_recommendations","Concierge__enable_remove_info_panel_tags","Profiles__enable_page_saving","Profiles__enable_callout_notifications","Profiles__enable_complete_playlist_endpoint","DevPro__enable_vertex_credit_card","Analytics__enable_devpro_interaction_logging","MiscFeatureFlags__developers_footer_dark_image","Profiles__enable_awarding_url","MiscFeatureFlags__fix_lower_breadcrumbs","DevPro__enable_credits_banner","DevPro__enable_google_payments_buyflow","Profiles__enable_developer_profile_benefits_ui_redesign","DevPro__enable_embed_profile_creation","DevPro__enable_enterprise","Profiles__enable_auto_apply_credits","DevPro__enable_devpro_offers","EngEduTelemetry__enable_engedu_telemetry","Search__enable_ai_eligibility_checks","Search__enable_suggestions_from_borg","MiscFeatureFlags__remove_cross_domain_tracking_params","Concierge__enable_actions_menu","Profiles__enable_join_program_group_endpoint","Profiles__enable_recognition_badges","MiscFeatureFlags__enable_explain_this_code","BookNav__enable_tenant_cache_key","Cloud__enable_llm_concierge_chat","MiscFeatureFlags__enable_framebox_badge_methods","DevPro__enable_g1_integration","DevPro__enable_developer_subscriptions","MiscFeatureFlags__developers_footer_image","Cloud__enable_free_trial_server_call","DevPro__remove_eu_tax_intake_form","MiscFeatureFlags__enable_appearance_cookies","Cloud__cache_serialized_dynamic_content","Experiments__reqs_query_experiments","Cloud__enable_legacy_calculator_redirect","DevPro__enable_google_one_card","Profiles__enable_public_developer_profiles","Search__enable_ai_search_summaries_for_all","MiscFeatureFlags__gdp_dashboard_reskin_enabled","Cloud__enable_cloudx_experiment_ids","TpcFeatures__enable_unmirrored_page_left_nav","Profiles__enable_developer_profile_pages_as_content","Profiles__enable_stripe_subscription_management","CloudShell__cloud_shell_button","DevPro__enable_cloud_innovators_plus","Concierge__enable_pushui","MiscFeatureFlags__enable_view_transitions","CloudShell__cloud_code_overflow_menu","Profiles__enable_user_type","SignIn__enable_l1_signup_flow","MiscFeatureFlags__enable_explicit_template_dependencies","DevPro__enable_code_assist","MiscFeatureFlags__enable_project_variables","DevPro__enable_nvidia_credits_card","TpcFeatures__proxy_prod_host","Profiles__enable_developer_profiles_callout","MiscFeatureFlags__enable_variable_operator","Concierge__enable_concierge_restricted","MiscFeatureFlags__enable_dark_theme","Profiles__enable_profile_collections","DevPro__enable_devsite_captcha","MiscFeatureFlags__enable_firebase_utm","OnSwitch__enable","Profiles__require_profile_eligibility_for_signin","MiscFeatureFlags__enable_variable_operator_index_yaml","Profiles__enable_targeted_hero","Cloud__fast_free_trial","Profiles__enable_purchase_prompts","Cloud__enable_cloud_shell","Concierge__enable_devsite_llm_tools","Analytics__enable_clearcut_logging","Cloud__enable_cloud_shell_fte_user_flow","DevPro__enable_free_benefits","Profiles__enable_playlist_community_acl","Cloud__enable_cloud_dlp_service","Profiles__enable_completequiz_endpoint","Search__enable_page_map","DevPro__enable_firebase_workspaces_card"],null,null,"AIzaSyBLEMok-5suZ67qRPzx0qUtbnLmyT_kCVE","https://developerscontentserving-pa.clients6.google.com","AIzaSyCM4QpTRSqP5qI4Dvjt4OAScIN8sOUlO-k","https://developerscontentsearch-pa.clients6.google.com",1,4,1,"https://developerprofiles-pa.clients6.google.com",[39,"clouddocs","Google Cloud Documentation","docs.cloud.google.com",null,"clouddocs-dot-devsite-v2-prod.appspot.com",null,null,[1,1,null,null,null,null,null,null,null,null,null,[1],null,null,null,null,null,1,[1],[null,null,null,[1,20],"/terms/recommendations"],[1],null,[1],[1,null,1],null,[1]],null,[54,null,null,null,null,null,"/images/lockup.svg","/images/favicons/onecloud/apple-icon.png",null,null,null,1,1,1,1,null,[],null,null,[[],[],[],[],[],[],[],[]],null,1,null,null,null,"/images/lockup-dark-theme.svg",[]],[],null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,null,[6,1,14,15,22,23,29,37],null,[[null,null,null,null,null,null,[1,[["docType","Choose a content type",[["ApiReference",null,null,null,null,null,null,null,null,"API reference"],["Sample",null,null,null,null,null,null,null,null,"Code sample"],["ReferenceArchitecture",null,null,null,null,null,null,null,null,"Reference architecture"],["Tutorial",null,null,null,null,null,null,null,null,"Tutorial"]]],["category","Choose a topic",[["AiAndMachineLearning",null,null,null,null,null,null,null,null,"Artificial intelligence and machine learning (AI/ML)"],["ApplicationDevelopment",null,null,null,null,null,null,null,null,"Application development"],["BigDataAndAnalytics",null,null,null,null,null,null,null,null,"Big data and analytics"],["Compute",null,null,null,null,null,null,null,null,"Compute"],["Containers",null,null,null,null,null,null,null,null,"Containers"],["Databases",null,null,null,null,null,null,null,null,"Databases"],["HybridCloud",null,null,null,null,null,null,null,null,"Hybrid and multicloud"],["LoggingAndMonitoring",null,null,null,null,null,null,null,null,"Logging and monitoring"],["Migrations",null,null,null,null,null,null,null,null,"Migrations"],["Networking",null,null,null,null,null,null,null,null,"Networking"],["SecurityAndCompliance",null,null,null,null,null,null,null,null,"Security and compliance"],["Serverless",null,null,null,null,null,null,null,null,"Serverless"],["Storage",null,null,null,null,null,null,null,null,"Storage"]]]]]],[1],null,1],[[null,null,null,null,null,["GTM-5CVQBG"],null,null,null,null,null,[["GTM-5CVQBG",2]],1],null,null,null,null,null,1],"mwETRvWii0eU5NUYprb0Y9z5GVbc",4],null,"pk_live_5170syrHvgGVmSx9sBrnWtA5luvk9BwnVcvIi7HizpwauFG96WedXsuXh790rtij9AmGllqPtMLfhe2RSwD6Pn38V00uBCydV4m",1,null,"https://developerscontentinsights-pa.clients6.google.com","AIzaSyCg-ZUslalsEbXMfIo9ZP8qufZgo3LSBDU","AIzaSyDxT0vkxnY_KeINtA4LSePJO-4MAZPMRsE","https://developers.clients6.google.com",null,null,"AIzaSyBQom12tzI-rybN7Sf-KfeL4nwm-Rf7PmI\n",null,null,"https://developers.googleapis.com"]')
  
</script>

    <devsite-a11y-announce></devsite-a11y-announce>
  </body>
</html>
