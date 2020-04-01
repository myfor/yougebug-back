const REDIRECT_TO = 'redirect-to';

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
function disabled(selector, value = undefined) {
    const DISABLED = 'disabled';
    const ELE = document.getElementById(selector);
    if (ELE.hasAttribute(DISABLED))
        return;
    ELE.setAttribute(DISABLED, DISABLED);
    if (value)
        ELE.value = value;
}

//  设置元素 enabled
function enabled(selector, value = undefined) {
    const DISABLED = 'disabled';
    const ELE = document.getElementById(selector);
    ELE.removeAttribute(DISABLED);
    if (value)
        ELE.value = value;
}

const LOGGED_KEY = '____';
//  当前是否有登录
function isLogged() {
    const IS_LOGGED = document.cookie.indexOf('8d0de8f8-61ef-4c56-a23e-de69a5f41681') !== -1;
    const current = localStorage.getItem(LOGGED_KEY);
    if (IS_LOGGED && current)
        return current;
    return null;
}
//  登出
function setLogout() {
    localStorage.removeItem('____');
    location.reload();
}
//  设置登录状态
//  value 是序列化后的
function setLogged(value) {
    localStorage.setItem(LOGGED_KEY, value);
    setUserAvatar(value);
}
//  重设登录状态
//  name 是名字
function reSetLogged(name) {
    const CURRENT = isLogged();
    if (CURRENT) {
        const DATA = JSON.parse(CURRENT);
        DATA.name = name;
        setLogged(JSON.stringify(DATA));
    }
}

function setUserAvatar(value) {
    if (!isLogged())
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
    if (hasAlert)
        history.replaceState(null, '', location.pathname);
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
            alert('请先登录');
        }
    } else {
        alert('系统错误，请重试');
    }
}