Feature: Sign in Sign up page
    As a user
    I want to click all buttons on SignInSignUp page

Background:
    Given I go to the website "/"
    Given I clicked on the element "a=Sign in/sign up"

Scenario: I am on SignInSignUp page 
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: Click Logo
    When I click on the element ".header-logo"
    Then I expect the title of the page "Welcome - UBORA"

Scenario: Click Sign in/sign up
    When I click on the element "a=Sign in/sign up"
    Then I expect the title of the page "Sign in to UBORA - UBORA"

Scenario: Click Sign up
    When I click on the element "a=Sign up"
    Then I expect the title of the page "Sign up - UBORA"

Scenario: Click Forgot password?
    When I click on the element "a=Forgot password?"
    Then I expect the title of the page "Forgot your password? - UBORA"
    When I set value "change@password.com" to the element "#Email"
    When I click on the element "button=Submit"
    Then I expect the title of the page "Forgot Password Confirmation - UBORA"

Scenario: I sign in without credentials
    When I click on the element "button=Sign in"
    Then I expect the element "span=The Email field is required." is visible
    Then I expect the element "span=The Password field is required." is visible