
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: undefined,
  entryPointToBrowserMapping: {},
  assets: {
    'index.csr.html': {size: 9182, hash: 'dd4a263a3713e76e22dac4bcf76eafe8605edbbb47407516724e8cec85dae11f', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1116, hash: '960251a6699250c4254519b72d622638b6221dd2f6775956900337af9f150a20', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'styles-JVNHWSQE.css': {size: 8755, hash: '6OQThRAm/hU', text: () => import('./assets-chunks/styles-JVNHWSQE_css.mjs').then(m => m.default)}
  },
};
