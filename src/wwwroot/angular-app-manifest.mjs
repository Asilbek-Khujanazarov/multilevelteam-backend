
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: undefined,
  entryPointToBrowserMapping: {},
  assets: {
    'index.csr.html': {size: 9175, hash: '5845ee57b32319420051785cc7e4bbb890273d24c2a334c90f5f8c5440b6ecdb', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1109, hash: '28a55dfbbe79f8bc21e3d6030b0e8439f5d8d0a63e81f3f0ac366a89a402e91d', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'styles-JVNHWSQE.css': {size: 8755, hash: '6OQThRAm/hU', text: () => import('./assets-chunks/styles-JVNHWSQE_css.mjs').then(m => m.default)}
  },
};
