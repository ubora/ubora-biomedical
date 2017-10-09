import MarkdownRenderer from './modules/markdown_renderer';
import MarkdownEditor from './modules/markdown_editor';
import Autocomplete from './modules/autocomplete';
import CopyToClipboard from './modules/clipboard';
import Feedback from './modules/feedback';
import Sidemenu from './modules/sidemenu';
import Notices from './modules/notices';

const styles = require('../Styles/styles.css').toString();

if (module.hot) {
  module.hot.accept();
}



window.addEventListener('load', () => {
  console.info(`UBORA: page loaded in ${Math.ceil(window.performance.now())}ms / target <500ms`);
});
