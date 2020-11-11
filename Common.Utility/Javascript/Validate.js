/*是否合法IP地址*/
export function validateIP(rule, value, callback) {
    if (value == '' || value == undefined || value == null) {
        callback();
    } else {
        const reg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$/;
        if ((!reg.test(value)) && value != '') {
            callback(new Error('请输入正确的IP地址'));
        } else {
            callback();
        }
    }
}

/* 是否手机号码或者固话*/
export function validatePhoneTwo(rule, value, callback) {
    const reg = /^((0\d{2,3}-\d{7,8})|(1[34578]\d{9}))$/;;
    if (value == '' || value == undefined || value == null) {
        callback();
    } else {
        if ((!reg.test(value)) && value != '') {
            callback(new Error('请输入正确的电话号码或者固话号码'));
        } else {
            callback();
        }
    }
}
/* 是否固话*/
export function validateTelphone(rule, value, callback) {
    const reg = /0\d{2}-\d{7,8}/;
    if (value == '' || value == undefined || value == null) {
        callback();
    } else {
        if ((!reg.test(value)) && value != '') {
            callback(new Error('请输入正确的固话（格式：区号+号码,如010-1234567）'));
        } else {
            callback();
        }
    }
}
/* 是否手机号码*/
export function validatePhone(rule, value, callback) {
    const reg = /^[1][3,4,5,7,8][0-9]{9}$/;
    if (value == '' || value == undefined || value == null) {
        callback();
    } else {
        if ((!reg.test(value)) && value != '') {
            callback(new Error('请输入正确的电话号码'));
        } else {
            callback();
        }
    }
}
/* 是否身份证号码*/
export function validateIdNo(rule, value, callback) {
    const reg = /(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)/;
    if (value == '' || value == undefined || value == null) {
        callback();
    } else {
        if ((!reg.test(value)) && value != '') {
            callback(new Error('请输入正确的身份证号码'));
        } else {
            callback();
        }
    }
}
/* 是否邮箱*/
export function validateEMail(rule, value, callback) {
    const reg = /^([a-zA-Z0-9]+[-_\.]?)+@[a-zA-Z0-9]+\.[a-z]+$/;
    if (value == '' || value == undefined || value == null) {
        callback();
    } else {
        if (!reg.test(value)) {
            callback(new Error('请输入正确的邮箱地址'));
        } else {
            callback();
        }
    }
}
/* 合法uri*/
export function validateURL(textval) {
    const urlregex = /^(https?|ftp):\/\/([a-zA-Z0-9.-]+(:[a-zA-Z0-9.&%$-]+)*@)*((25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])){3}|([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{2}))(:[0-9]+)*(\/($|[a-zA-Z0-9.,?'\\+&%$#=~_-]+))*$/;
    return urlregex.test(textval);
}

/*验证内容是否英文数字以及下划线*/
export function isPassword(rule, value, callback) {
    const reg = /^[_a-zA-Z0-9]+$/;
    if (value == '' || value == undefined || value == null) {
        callback();
    } else {
        if (!reg.test(value)) {
            callback(new Error('密码仅由英文字母，数字以及下划线组成'));
        } else {
            callback();
        }
    }
}

/*自动检验数值的范围*/
export function checkMax20000(rule, value, callback) {
    if (value == '' || value == undefined || value == null) {
        callback();
    } else if (!Number(value)) {
        callback(new Error('请输入[1,20000]之间的数字'));
    } else if (value < 1 || value > 20000) {
        callback(new Error('请输入[1,20000]之间的数字'));
    } else {
        callback();
    }
}

//验证数字输入框最大数值,32767
export function checkMaxVal(rule, value, callback) {
    if (value < 0 || value > 32767) {
        callback(new Error('请输入[0,32767]之间的数字'));
    } else {
        callback();
    }
}
//验证是否1-99之间
export function isOneToNinetyNine(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'));
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入正整数'));
        } else {
            const re = /^[1-9][0-9]{0,1}$/;
            const rsCheck = re.test(value);
            if (!rsCheck) {
                callback(new Error('请输入正整数，值为【1,99】'));
            } else {
                callback();
            }
        }
    }, 0);
}

// 验证是否整数
export function isInteger(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'));
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入正整数'));
        } else {
            const re = /^[0-9]*[1-9][0-9]*$/;
            const rsCheck = re.test(value);
            if (!rsCheck) {
                callback(new Error('请输入正整数'));
            } else {
                callback();
            }
        }
    }, 0);
}
// 验证是否整数,非必填
export function isIntegerNotMust(rule, value, callback) {
    if (!value) {
        callback();
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入正整数'));
        } else {
            const re = /^[0-9]*[1-9][0-9]*$/;
            const rsCheck = re.test(value);
            if (!rsCheck) {
                callback(new Error('请输入正整数'));
            } else {
                callback();
            }
        }
    }, 1000);
}

// 验证是否是[0-1]的小数
export function isDecimal(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'));
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入[0,1]之间的数字'));
        } else {
            if (value < 0 || value > 1) {
                callback(new Error('请输入[0,1]之间的数字'));
            } else {
                callback();
            }
        }
    }, 100);
}

// 验证是否是[1-10]的小数,即不可以等于0
export function isBtnOneToTen(rule, value, callback) {
    if (typeof value == 'undefined') {
        return callback(new Error('输入不可以为空'));
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入正整数，值为[1,10]'));
        } else {
            if (!(value == '1' || value == '2' || value == '3' || value == '4' || value == '5' || value == '6' || value == '7' || value == '8' || value == '9' || value == '10')) {
                callback(new Error('请输入正整数，值为[1,10]'));
            } else {
                callback();
            }
        }
    }, 100);
}
// 验证是否是[1-100]的小数,即不可以等于0
export function isBtnOneToHundred(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'));
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入整数，值为[1,100]'));
        } else {
            if (value < 1 || value > 100) {
                callback(new Error('请输入整数，值为[1,100]'));
            } else {
                callback();
            }
        }
    }, 100);
}
// 验证是否是[0-100]的小数
export function isBtnZeroToHundred(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'));
    }
    setTimeout(() => {
        if (!Number(value)) {
            callback(new Error('请输入[1,100]之间的数字'));
        } else {
            if (value < 0 || value > 100) {
                callback(new Error('请输入[1,100]之间的数字'));
            } else {
                callback();
            }
        }
    }, 100);
}

// 验证端口是否在[0,65535]之间
export function isPort(rule, value, callback) {
    if (!value) {
        return callback(new Error('输入不可以为空'));
    }
    setTimeout(() => {
        if (value == '' || typeof (value) == undefined) {
            callback(new Error('请输入端口值'));
        } else {
            const re = /^([0-9]|[1-9]\d|[1-9]\d{2}|[1-9]\d{3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$/;
            const rsCheck = re.test(value);
            if (!rsCheck) {
                callback(new Error('请输入在[0-65535]之间的端口值'));
            } else {
                callback();
            }
        }
    }, 100);
}
// 验证端口是否在[0,65535]之间，非必填,isMust表示是否必填
export function isCheckPort(rule, value, callback) {
    if (!value) {
        callback();
    }
    setTimeout(() => {
        if (value == '' || typeof (value) == undefined) {
            //callback(new Error('请输入端口值'));
        } else {
            const re = /^([0-9]|[1-9]\d|[1-9]\d{2}|[1-9]\d{3}|[1-5]\d{4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])$/;
            const rsCheck = re.test(value);
            if (!rsCheck) {
                callback(new Error('请输入在[0-65535]之间的端口值'));
            } else {
                callback();
            }
        }
    }, 100);
}

/* 小写字母*/
export function validateLowerCase(str) {
    const reg = /^[a-z]+$/;
    return reg.test(str);
}
/*保留2为小数*/
export function validatetoFixedNew(str) {
    return str;
}
/* 验证key*/
// export function validateKey(str) {
//     var reg = /^[a-z_\-:]+$/;
//     return reg.test(str);
// }

/* 大写字母*/
export function validateUpperCase(str) {
    const reg = /^[A-Z]+$/;
    return reg.test(str);
}

/* 大小写字母*/
export function validatAlphabets(str) {
    const reg = /^[A-Za-z]+$/;
    return reg.test(str);
}
// 判断是否为空的正则
export function isSpace(str) {
    const reg = /(^\s+)|(\s+$)|\s+/g
    return reg.test(str)
}
/**
 * 匹配double或float
 */
export function isDouble(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^[-\+]?\d+(\.\d+)?$/);
    if (result == null) return false;
    return true;
}
/**
 * 匹配邮政编码
 */
export function isZipCode(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^[0-9]{6}$/);
    if (result == null) return false;
    return true;
}

/**
 * 匹配URL
 */
export function isUrl(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\’:+!]*([^<>\"])*$/);
    if (result == null) return false;
    return true;
}

/**
 * 匹配密码，以字母开头，长度在6-12之间，只能包含字符、数字和下划线。
 */
export function isPwd(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^[a-zA-Z]\\w{6,12}$/);
    if (result == null) return false;
    return true;
}

/**
 * 判断是否为合法字符(a-zA-Z0-9-_)
 */
export function isRightfulString(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^[A-Za-z0-9_-]+$/);
    if (result == null) return false;
    return true;
}

/**
 * 匹配english
 */
export function isEnglish(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^[A-Za-z]+$/);
    if (result == null) return false;
    return true;
}

/**
 * 匹配身份证号码
 */
export function isIdCardNo(num) {
    //　 if (isNaN(num)) {alert("输入的不是数字！"); return false;}
    var len = num.length, re;
    if (len == 15)
        re = new RegExp(/^(\d{6})()?(\d{2})(\d{2})(\d{2})(\d{2})(\w)$/);
    else if (len == 18)
        re = new RegExp(/^(\d{6})()?(\d{4})(\d{2})(\d{2})(\d{3})(\w)$/);
    else { alert("输入的数字位数不对。"); return false; }
    var a = num.match(re);
    if (a != null) {
        if (len == 15) {
            var D = new Date("19" + a[3] + "/" + a[4] + "/" + a[5]);
            var B = D.getYear() == a[3] && (D.getMonth() + 1) == a[4] && D.getDate() == a[5];
        }
        else {
            var D = new Date(a[3] + "/" + a[4] + "/" + a[5]);
            var B = D.getFullYear() == a[3] && (D.getMonth() + 1) == a[4] && D.getDate() == a[5];
        }
        if (!B) { alert("输入的身份证号 " + a[0] + " 里出生日期不对。"); return false; }
    }
    if (!re.test(num)) { alert("身份证最后一位只能是数字和字母。"); return false; }

    return true;
}

/**
 * 匹配汉字
 */
export function isChinese(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^[\u4e00-\u9fa5]+$/);
    if (result == null) return false;
    return true;
}

/**
 * 匹配中文(包括汉字和字符)
 */
export function isChineseChar(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^[\u0391-\uFFE5]+$/);
    if (result == null) return false;
    return true;
}

/**
 * 字符验证，只能包含中文、英文、数字、下划线等字符。
 */
export function stringCheck(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^[a-zA-Z0-9\u4e00-\u9fa5-_]+$/);
    if (result == null) return false;
    return true;
}

/**
 * 过滤中英文特殊字符，除英文"-_"字符外
 */
export function stringFilter(str) {
    var pattern = new RegExp("[`~!@#$%^&*()+=|{}':;',\\[\\].<>/?~！@#￥%……&*（）——+|{}【】‘；：”“’。，、？]");
    var rs = "";
    for (var i = 0; i < str.length; i++) {
        rs = rs + str.substr(i, 1).replace(pattern, '');
    }
    return rs;
}

/**
 * 判断是否包含中英文特殊字符，除英文"-_"字符外
 */
export function isContainsSpecialChar(str) {
    if (str == null || str == "") return false;
    var reg = RegExp(/[(\ )(\`)(\~)(\!)(\@)(\#)(\$)(\%)(\^)(\&)(\*)(\()(\))(\+)(\=)(\|)(\{)(\})(\')(\:)(\;)(\')(',)(\[)(\])(\.)(\<)(\>)(\/)(\?)(\~)(\！)(\@)(\#)(\￥)(\%)(\…)(\&)(\*)(\（)(\）)(\—)(\+)(\|)(\{)(\})(\【)(\】)(\‘)(\；)(\：)(\”)(\“)(\’)(\。)(\，)(\、)(\？)]+/);
    return reg.test(str);
}
/**
 * 判断整数num是否等于0
 *
 * @param num
 * @return
 * @author jiqinlin
 */
export function isIntEqZero(num) {
    return num == 0;
}

/**
 * 判断整数num是否大于0
 *
 * @param num
 * @return
 * @author jiqinlin
 */
export function isIntGtZero(num) {
    return num > 0;
}

/**
 * 判断整数num是否大于或等于0
 *
 * @param num
 * @return
 * @author jiqinlin
 */
export function isIntGteZero(num) {
    return num >= 0;
}

/**
 * 判断浮点数num是否等于0
 *
 * @param num 浮点数
 * @return
 * @author jiqinlin
 */
export function isFloatEqZero(num) {
    return num == 0;
}

/**
 * 判断浮点数num是否大于0
 *
 * @param num 浮点数
 * @return
 * @author jiqinlin
 */
export function isFloatGtZero(num) {
    return num > 0;
}

/**
 * 判断浮点数num是否大于或等于0
 *
 * @param num 浮点数
 * @return
 * @author jiqinlin
 */
export function isFloatGteZero(num) {
    return num >= 0;
}

/**
 * 匹配Email地址
 */
export function isEmail(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/);
    if (result == null) return false;
    return true;
}

/**
 * 判断数值类型，包括整数和浮点数
 */
export function isNumber(str) {
    if (isDouble(str) || isInteger(str)) return true;
    return false;
}

/**
 * 只能输入数字[0-9]
 */
export function isDigits(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^\d+$/);
    if (result == null) return false;
    return true;
}

/**
 * 匹配money
 */
export function isMoney(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^(([1-9]\d*)|(([0-9]{1}|[1-9]+)\.[0-9]{1,2}))$/);
    if (result == null) return false;
    return true;
}

/**
 * 匹配phone
 */
export function isPhone(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/);
    if (result == null) return false;
    return true;
}

/**
 * 匹配mobile
 */
export function isMobile(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^((\(\d{2,3}\))|(\d{3}\-))?((13\d{9})|(15\d{9})|(18\d{9}))$/);
    if (result == null) return false;
    return true;
}

/**
 * 联系电话(手机/电话皆可)验证
 */
export function isTel(String text) {
    if (isMobile(text) || isPhone(text)) return true;
    return false;
}

/**
 * 匹配qq
 */
export function isQq(str) {
    if (str == null || str == "") return false;
    var result = str.match(/^[1-9]\d{4,12}$/);
    if (result == null) return false;
    return true;
}

/**
   *
   * 判断是否是图片
   * @static
   * @param {*} path
   * @returns
   * @memberof Utils
   */
export function isImg(path) {
    let reg = /.+(.JPEG|.jpeg|.JPG|.jpg|.GIF|.gif|.BMP|.bmp|.PNG|.png)$/;
    return reg.test(path);
}