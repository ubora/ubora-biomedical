Feature: User password changes
    As a user
    I want to register an account and change my password

Background:
    Given I go to Home page

Scenario: I sign in as user
    When I sign in as user
    Then I expect the title of the page "Welcome - UBORA"

Scenario: I try to change my password to empty password
    When I click on the element "span=Menu"
    And I click on the element "a=View profile"
    Then I expect the title of the page "Manage your account - UBORA"
    When I click on the element "a=Change password"
    Then I expect the title of the page "Change Password - UBORA"
    When I click on the element "button=Change password"
    Then I expect the element "span=The Old password field is required." is visible
    And I expect the element "span=The New password field is required." is visible
    And I expect the title of the page "Change Password - UBORA"

Scenario: I change my password
    When I click on the element "span=Menu"
    And I click on the element "a=View profile"
    And I click on the element "a=Change password"
    And I set value "ChangeMe123!" to the element "#OldPassword"
    And I set value "ChangeMe321!" to the element "#NewPassword"
    And I set value "ChangeMe321!" to the element "#ConfirmPassword"
    And I click on the element "button=Change password"
    Then I expect the title of the page "Manage your account - UBORA"

Scenario: I log out
    When I log out
    Then I expect the title of the page "Welcome - UBORA"
    And I expect the element "#SignInSignUp" is visible

Scenario: I log in with my changed password
    When I sign in as "test@agileworks.eu" with password "ChangeMe321!"
    Then I expect the title of the page "Welcome - UBORA"
    When I click on the element "span=Menu"
    Then I expect the element "p=Change Password" is visible
