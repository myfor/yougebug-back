
/**
 * 提示弹窗
 * @param content 提示内容
 * @param title 提示标题
 * @param clickFunction 按钮事件，传入要执行的方法体
 */
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

    DIV_3.innerHTML = `
            <h5 class="modal-title" id="exampleModalLabel">${title ? title : "提示"}</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
`;
    const DIV_4 = DOC.createElement('div');
    DIV_4.className = 'modal-body';
    DIV_4.innerText = content;

    const DIV_FOOTER = DOC.createElement('div');
    DIV_FOOTER.className = `modal-footer`;

    const BTN_CLOSE = DOC.createElement('button');
    BTN_CLOSE.type = 'button';
    BTN_CLOSE.className = 'btn btn-out-secondary';
    BTN_CLOSE.setAttribute("data-dismiss", "modal");
    BTN_CLOSE.innerText = '关闭';

    DIV_FOOTER.appendChild(BTN_CLOSE);
    if (clickFunction) {
        const BTN_CLICK = DOC.createElement('button');
        BTN_CLICK.type = 'button';
        BTN_CLICK.className = 'btn btn-primary';
        BTN_CLICK.innerText = '确定';
        BTN_CLICK.onclick = clickFunction;

        DIV_FOOTER.appendChild(BTN_CLICK);
    }

    TOP.appendChild(DIV_1);
    DIV_1.appendChild(DIV_2);
    DIV_2.appendChild(DIV_3);
    DIV_2.appendChild(DIV_4);
    DIV_2.appendChild(DIV_FOOTER);

    $(TOP).modal('show');
}

const GO_TO = 'redirect-to';

function getQueryString(name) {
    var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)', 'i');
    var r = window.location.search.substr(1).match(reg);
    if (r != null) {
        return decodeURI(r[2]);
    }
    return '';
}

//  验证邮箱格式
function checkEmail(email) {
    email = email.trim();
    let reg = new RegExp("^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$");
    return reg.test(email);
}

//  设置元素 disabled
function disabled(selector, value) {
    const DISABLED = 'disabled';
    const ELE = document.getElementById(selector);
    if (ELE.hasAttribute(DISABLED))
        return;
    ELE.setAttribute(DISABLED, DISABLED);
    if (value) {
        switch (ELE.nodeName) {
            case 'BUTTON': ELE.innerHTML = value; break;
            default: ELE.value = value; break;
        }
    }
}

//  设置元素 enabled
function enabled(selector, value) {
    const DISABLED = 'disabled';
    const ELE = document.getElementById(selector);
    ELE.removeAttribute(DISABLED);
    if (value) {
        switch (ELE.nodeType) {
            case 'BUTTON': ELE.innerHTML = value; break;
            default: ELE.value = value; break;
        }
    }
}

//  当前是否已经登录的缓存
//  获取当前用户是否登录的缓存
function getLoggedCache() {
    return localStorage.getItem('_LoggedCache_');
}
//  设置当前用户是否登录的缓存
function setLoggedCache(loggedCache) {
    localStorage.setItem('_LoggedCache_', loggedCache);
}
const LOGGED_KEY = '____';
//  当前是否有登录
async function isLoggedAsync() {
    let _isLogged;

    await axios.get('/islogged')
        .then(function (resp) {
            if (resp.status === 200) {
                _isLogged = true;
            } else {
                _isLogged = false;
            }
        })
        .catch(function (err) {
            catchErr(err);
            _isLogged = false;
        });

    const current = localStorage.getItem(LOGGED_KEY);
    if (_isLogged && current)
        return current;
    return null;
}
//  登出
function setLogout() {
    localStorage.removeItem(LOGGED_KEY);
    location.reload();
}
//  设置登录状态
//  value 是序列化后的
function setLogged(value) {
    localStorage.setItem(LOGGED_KEY, value);
    setUserAvatar(value);
    setLoggedCache(value);
}
//  重设登录状态
//  name 是名字
function reSetLogged(name) {
    const CURRENT = getLoggedCache();
    if (CURRENT) {
        const DATA = JSON.parse(CURRENT);
        DATA.name = name;
        setLogged(JSON.stringify(DATA));
    }
}

function setUserAvatar(value) {
    if (!getLoggedCache())
        return;
    const INFO = JSON.parse(value);
    const AVATAR = $('#img_avatar');
    AVATAR.src = INFO.avatar;
    AVATAR.alt = INFO.name;
}

//  普通通知
const ALERT_PRIMARY = 'alert-primary';
//  警告通知
const ALERT_WARNING = 'alert-warning';
//  检查是否有警告
function checkAlert() {

    let MSG = getQueryString(ALERT_PRIMARY);
    let hasAlert = false;
    if (MSG) {
        $('#d_alert').html($('#d_alert').html() + getAlertMode('primary', MSG));
        hasAlert = true;
    }
    MSG = getQueryString(ALERT_WARNING);
    if (MSG) {
        $('#d_alert').html($('#d_alert').html() + getAlertMode('warning', MSG));
        hasAlert = true;
    }
    if (hasAlert) {
        MSG = getQueryString(GO_TO);
        let pathName = '';
        if (MSG)
            pathName = `?${GO_TO}=${MSG}`;
        history.replaceState(null, '', location.pathname + pathName);
    }
}
function getAlertMode(type, msg) {
    return `
<div class="alert alert-${type} alert-dismissible fade show" role="alert" style="margin-bottom: auto;">
${msg}
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>
`;
}

function catchErr(err) {
    if (err.response) {
        if (err.response.status === 401) {
            setLogout();
            showAlert('请先登录');
        }
    } else {
        showAlert('系统错误，请重试');
    }
}