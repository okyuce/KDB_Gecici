"use strict";
var KTSigninGeneral = function() {
	var t, e, r;
	return {
		init: function() {
			t = document.querySelector("#kt_sign_in_form"), e = document.querySelector("#kt_sign_in_submit"), r = FormValidation.formValidation(t, {
				fields: {
					email: {
						validators: {
							regexp: {
								regexp: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
								message: "The value is not a valid email address"
							},
							notEmpty: {
								message: "'Kullan�c� Ad�' alan� zorunludur."
							}
						}
					},
					password: {
						validators: {
							notEmpty: {
								message: "'�ifre' alan� zorunludur."
							}
						}
					}
				},
				plugins: {
					trigger: new FormValidation.plugins.Trigger,
					bootstrap: new FormValidation.plugins.Bootstrap5({
						rowSelector: ".fv-row",
						eleInvalidClass: "",
						eleValidClass: ""
					})
				}
			}), ! function(t) {
				try {
					return new URL(t), !0
				} catch (t) {
					return !1
				}
			}
				(e.closest("form").getAttribute("action")) ? e.addEventListener("click", (function (i) {
				i.preventDefault(), r.validate().then((function(r) {
					"Valid" == r ? (e.setAttribute("data-kt-indicator", "on"), e.disabled = !0, setTimeout((function() {
						e.removeAttribute("data-kt-indicator"), e.disabled = !1, Swal.fire({
							text: "You have successfully logged in!",
							icon: "success",
							buttonsStyling: !1,
							confirmButtonText: "Ok, got it!",
							customClass: {
								confirmButton: "btn btn-primary"
							}
						}).then((function(e) {
							if (e.isConfirmed) {
								t.querySelector('[name="KullaniciAdi"]').value = "", t.querySelector('[name="Sifre"]').value = "";
								var r = t.getAttribute("data-kt-redirect-url");
								r && (location.href = r)
							}
						}))
					}), 2e3)) : Swal.fire({
						text: "Sorry, looks like there are some errors detected, please try again.",
						icon: "error",
						buttonsStyling: !1,
						confirmButtonText: "Ok, got it!",
						customClass: {
							confirmButton: "btn btn-primary"
						}
					})
				}))
			})) : e.addEventListener("click", (function(i) {
				i.preventDefault(), r.validate().then((function(r) {
					"Valid" == r ? (e.setAttribute("data-kt-indicator", "on"), e.disabled = !0, axios.post(e.closest("form").getAttribute("action"), new FormData(t)).then((function(e) {
						if (e) {
							t.reset(), Swal.fire({
								text: "Ba�ar�yla giri� yap�ld�.",
								icon: "success",
								buttonsStyling: !1,
								confirmButtonText: "Ok, got it!",
								customClass: {
									confirmButton: "btn btn-primary"
								}
							});
							const e = t.getAttribute("data-kt-redirect-url");
							e && (location.href = e)
						} else Swal.fire({
							text: "'Kullan�c� Ad�' ya da '�ifre' hatal�.",
							icon: "error",
							buttonsStyling: !1,
							confirmButtonText: "Ok, got it!",
							customClass: {
								confirmButton: "btn btn-primary"
							}
						})
					})).catch((function(t) {
						Swal.fire({
							text: "Sorry, looks like there are some errors detected, please try again.",
							icon: "error",
							buttonsStyling: !1,
							confirmButtonText: "Ok, got it!",
							customClass: {
								confirmButton: "btn btn-primary"
							}
						})
					})).then((() => {
						e.removeAttribute("data-kt-indicator"), e.disabled = !1
					}))) : Swal.fire({
						text: "Sorry, looks like there are some errors detected, please try again.",
						icon: "error",
						buttonsStyling: !1,
						confirmButtonText: "Ok, got it!",
						customClass: {
							confirmButton: "btn btn-primary"
						}
					})
				}))
			}))
		}
	}
}();

KTUtil.onDOMContentLoaded((function() {
	KTSigninGeneral.init()
}));