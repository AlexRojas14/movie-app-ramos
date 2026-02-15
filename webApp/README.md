# MovieAppRamos - Frontend (`webApp`)

Aplicacion frontend en React + TypeScript + Vite.

Actualmente implementa un tablero Kanban con:

- drag and drop (`@dnd-kit`)
- estado global con `zustand`
- persistencia en `localStorage` (`kanban-board-state`)
- pruebas con `vitest` + Testing Library
- componentes documentados en Storybook

## Stack

- React 19
- TypeScript 5
- Vite 7
- Tailwind CSS
- Vitest
- Storybook 10

## Requisitos

- Node.js 20+ (recomendado LTS actual)
- npm 10+

## Instalacion

Desde la carpeta `webApp`:

```bash
npm install
```

## Scripts disponibles

```bash
npm run dev
```

Inicia desarrollo en caliente (Vite).

```bash
npm run build
```

Compila TypeScript y genera build de produccion en `dist/`.

```bash
npm run preview
```

Sirve localmente la build de `dist/`.

```bash
npm run test
```

Ejecuta pruebas unitarias una vez.

```bash
npm run test:watch
```

Ejecuta Vitest en modo watch.

```bash
npm run storybook
```

Levanta Storybook en `http://localhost:6006`.

```bash
npm run build-storybook
```

Genera build estatico de Storybook en `storybook-static/`.

```bash
npm run lint
```

Ejecuta ESLint sobre el proyecto.

## Variables de entorno

Hoy no hay variables obligatorias para correr el frontend.

Si luego integras llamadas HTTP al backend, usa variables `VITE_*` en un archivo `.env`, por ejemplo:

```dotenv
VITE_API_BASE_URL=http://localhost:5252
```

## Pruebas

Se validaron correctamente las pruebas del frontend:

- 5 archivos de prueba
- 14 pruebas pasando

Comando:

```bash
npm run test
```

## Build y deploy

### Build local

```bash
npm run build
```

Salida: `webApp/dist`

### Vercel

Si el repo completo contiene backend y frontend, configura:

- Root Directory: `webApp`
- Install Command: `npm install` (o `npm ci`)
- Build Command: `npm run build`
- Output Directory: `dist`

## Estructura principal

- `src/app` entrada de la app
- `src/features/board` feature Kanban
- `src/shared` utilidades y componentes UI reutilizables
- `.storybook` configuracion de Storybook

## Nota sobre lint

Actualmente `npm run lint` reporta errores por:

- reglas de React hooks en algunos archivos fuente
- inclusion de artefactos `storybook-static` en el analisis

Si quieres, en el siguiente paso te lo puedo dejar limpio para que `npm run lint` pase en verde.
