import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// 这个配置文件会被 Aspire 注入的环境变量驱动：
// 1. VITE_API_BASE_URL 用来把前端开发请求代理到后端 API；
// 2. VITE_WEB_PORT 让 Vite 本身也使用固定端口；
// 3. strictPort=true 用来确保端口冲突时直接报错，而不是偷偷换成随机端口。
export default defineConfig(({ command }) => {
  const isServe = command === 'serve'
  const apiBaseUrl = process.env.VITE_API_BASE_URL
  const webPort = Number(process.env.VITE_WEB_PORT ?? '5173')

  if (isServe && !apiBaseUrl) {
    throw new Error('缺少 VITE_API_BASE_URL。请通过 Aspire 启动前端。')
  }

  return {
    plugins: [vue()],
    server: isServe
      ? {
          host: '0.0.0.0',
          port: webPort,
          strictPort: true,
          proxy: {
            '/api': {
              target: apiBaseUrl!,
              changeOrigin: true,
            },
            '/health': {
              target: apiBaseUrl!,
              changeOrigin: true,
            },
            '/swagger': {
              target: apiBaseUrl!,
              changeOrigin: true,
            },
          },
        }
      : undefined,
  }
})
