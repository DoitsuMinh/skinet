# STRIPE TEST WORKBENCH WEBHOOKS 

## 1. [Install Stripe CLI](https://docs.stripe.com/stripe-cli?install-method=scoop)

## 2. Log in to the CLi 

####  Log in and authenticate your Stripe user Account to generate a set of restricted keys. To learn more, see [Stripe CLI keys and permissions.](https://docs.stripe.com/stripe-cli/keys)

####
```Command Line
stripe login
```

####  (Optional) If you don’t want to use a browser, use the --interactive flag to authenticate with an existing API secret key or restricted key. This is helpful when authenticating to the CLI without a browser, such as in a CI/CD pipeline.
####
```Command Line
stripe login --interactive
```

## 3.  View the event 

####
```
stripe listen --forward-to https://localhost:5001/api/payments/webhook -e payment_intent.succeeded
```