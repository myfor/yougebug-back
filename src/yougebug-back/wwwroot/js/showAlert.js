function showAlert(content, title, clickFunction) {
    const DOC = document;

    const TOP = DOC.createElement('div');
    TOP.className = 'modal fade';
    TOP.id = 'mod_alert';
    TOP.setAttribute('tabindex', '-1');
    TOP.setAttribute('role', 'role');
    TOP.setAttribute('aria-labelledby', 'modelLay');
    TOP.setAttribute('aria-hidden', 'true');

    const DIV_1 = DOC.createElement('div');
    DIV_1.className = 'modal-dialog';
    DIV_1.setAttribute('role', 'cocument');

    const DIV_2 = DOC.createElement('div');
    DIV_2.className = 'modal-content';

    const DIV_3 = DOC.createElement('div');
    DIV_3.className = 'modal-header';

    const H_1 = DOC.createElement('h5');
    H_1.className = 'modal-title';
    H_1.id = "modelLay";
    H_1.innerText = title ? title : '提示';

    const BTN_CLOSE_iCON = DOC.createElement('button');
    BTN_CLOSE_iCON.type = 'button';
    BTN_CLOSE_iCON.className = 'close';
    BTN_CLOSE_iCON.setAttribute("data-dismiss", "modal");
    BTN_CLOSE_iCON.setAttribute("aria-label", "Close");
    BTN_CLOSE_iCON.innerHTML = `<span aria-hidden="true">&times;</span>`;

    const DIV_CONTENT = DOC.createElement('div');
    DIV_CONTENT.className = `modal-body`;
    DIV_CONTENT.innerText = content;

    const DIV_FOOTER = DOC.createElement('div');
    DIV_FOOTER.className = `modal-footer`;

    const BTN_CLOSE = DOC.createElement('button');
    BTN_CLOSE.type = 'button';
    BTN_CLOSE.className = 'btn btn-out-secondary';
    BTN_CLOSE.setAttribute("data-dismiss", "modal");
    BTN_CLOSE.innerText = '关闭';

    DIV_3.appendChild(H_1);
    DIV_3.appendChild(BTN_CLOSE_iCON);
}