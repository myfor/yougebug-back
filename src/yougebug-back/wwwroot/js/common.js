
/**
 * 提示弹窗
 * @param content 提示内容
 * @param title 提示标题
 * @param click 按钮事件，直接执行的JS代码
 */
function showAlert(content, title, click) {
    const MOD = document.createElement('div');
    MOD.className = 'modal fade';
    MOD.id = 'mod_alert';
    MOD.setAttribute('tabindex', '-1');
    MOD.setAttribute('role', 'role');
    MOD.setAttribute('aria-labelledby', 'exampleModalLabel');
    MOD.setAttribute('aria-hidden', 'true');

    const BTN = click ? `<button type="button" class="btn btn-primary" onclick="javascript:${click};">确定</button>` : '';

    MOD.innerHTML = `
<div class="modal-dialog" role="document">
    <div class="modal-content">
        <div class="modal-header">
            <h5 class="modal-title" id="exampleModalLabel">${title ? title : "提示"}</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <div class="modal-body">
        ${content}
        </div>
        <div class="modal-footer">
            <button type="button" class="btn btn-out-secondary" data-dismiss="modal">关闭</button>
            ${BTN}
        </div>
    </div>
 </div>
`;

    $(MOD).modal('show');
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
function disabled(selector, value, elementType) {
    const DISABLED = 'disabled';
    const ELE = document.getElementById(selector);
    if (ELE.hasAttribute(DISABLED))
        return;
    ELE.setAttribute(DISABLED, DISABLED);
    if (value) {
        switch (elementType) {
            case 'button': ELE.innerHTML = value; break;
            default: ELE.value = value; break;
        }
    }
}

//  设置元素 enabled
function enabled(selector, value, elementType) {
    const DISABLED = 'disabled';
    const ELE = document.getElementById(selector);
    ELE.removeAttribute(DISABLED);
    if (value) {
        switch (elementType) {
            case 'button': ELE.innerHTML = value; break;
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