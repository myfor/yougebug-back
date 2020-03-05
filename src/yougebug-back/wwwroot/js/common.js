
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
function disabled(selector) {
    const DISABLED = 'disabled';
    const ELE = document.getElementById(selector);
    if (ELE.hasAttribute(DISABLED))
        return;
    ELE.setAttribute(DISABLED, DISABLED);
}

//  设置元素 enabled
function enabled(selector) {
    const DISABLED = 'disabled';
    document.getElementById(selector).removeAttribute(DISABLED);
}
//  当前是否有登录
function isLogged() {
    const current = localStorage.getItem('____');
    return current;
}
//  检查是否有警告
function checkAlert() {
    const MSG = getQueryString('alert_warning');
    if (!MSG)
        return;

    let alertElement = `
<div class="alert alert-warning alert-dismissible fade show" role="alert">
${MSG}
  <button type="button" class="close" data-dismiss="alert" aria-label="Close">
    <span aria-hidden="true">&times;</span>
  </button>
</div>
`;
    $('#d_alert').html(alertElement);
}