
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: undefined,
  entryPointToBrowserMapping: {},
  assets: {
    'index.csr.html': {size: 9175, hash: '3ed10bce967c73d8c9c0098c5f072e774aaf6cc2bd28344e6fd721d7a5354045', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1109, hash: '9c31a4e61430f3ab88bb341f135a0a1a8d331a53fa131df5af13342c32820f5b', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'styles-JVNHWSQE.css': {size: 8755, hash: '6OQThRAm/hU', text: () => import('./assets-chunks/styles-JVNHWSQE_css.mjs').then(m => m.default)}
  },
};
