import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = localStorage.getItem('hortiloc_auth');

  if (!auth) {
    return next(req);
  }

  try {
    const utilisateur = JSON.parse(auth);
    const token = utilisateur?.token;

    if (!token) {
      return next(req);
    }

    const requete = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });

    return next(requete);
  }
  catch {
    return next(req);
  }
};