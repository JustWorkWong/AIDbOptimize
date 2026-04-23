/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_API_BASE_URL?: string
  readonly VITE_WEB_PORT?: string
  readonly VITE_PGADMIN_URL?: string
  readonly VITE_PHPMYADMIN_URL?: string
  readonly VITE_REDIS_INSIGHT_URL?: string
  readonly VITE_RABBITMQ_URL?: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}
