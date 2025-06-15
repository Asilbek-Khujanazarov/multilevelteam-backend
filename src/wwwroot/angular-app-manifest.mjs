
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: undefined,
  entryPointToBrowserMapping: {},
  assets: {
    'index.csr.html': {size: 9175, hash: 'bc1db8b4c0e144457e298fe891fea3166cc02109d84561c5ccdd9f543d3cce4e', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1109, hash: '578af92f6b9befefdced01f53319ef342db034af3e3377f614c4c42351ef5bb7', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'styles-JVNHWSQE.css': {size: 8755, hash: '6OQThRAm/hU', text: () => import('./assets-chunks/styles-JVNHWSQE_css.mjs').then(m => m.default)}
  },
};
